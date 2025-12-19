"use client";

import React, { useEffect, useRef } from "react";

const PortugueseFlagWebGL: React.FC = () => {
  const canvasRef = useRef<HTMLCanvasElement | null>(null);

  useEffect(() => {
    const canvas = canvasRef.current;
    if (!canvas) return;

    const gl = canvas.getContext("webgl");
    if (!gl) {
      console.error("WebGL not supported");
      return;
    }

    // ----------- SHADERS (simple, reliable) -----------
    const vertexSrc = `
      attribute vec3 a_position;
      attribute vec2 a_texCoord;

      uniform float u_time;

      varying vec2 v_texCoord;

      void main() {
        // base position in clipspace
        vec3 pos = a_position;

        // simple cloth-like wave, strongest on the free edge (right side)
        float edge = (pos.x + 1.0) * 0.5; // 0 at left, 1 at right
        float waveX = sin((pos.y * 4.0) + u_time * 2.0);
        float waveY = cos((pos.y * 3.0) - u_time * 1.5);

        float displacement = (waveX * 0.07 + waveY * 0.03) * edge;
        pos.x += displacement * 0.2;
        pos.y += displacement * 0.1;

        gl_Position = vec4(pos, 1.0);
        v_texCoord = a_texCoord;
      }
    `;

    const fragmentSrc = `
      precision mediump float;

      varying vec2 v_texCoord;
      uniform sampler2D u_texture;
      uniform int u_useTexture;

      void main() {
        if (u_useTexture == 1) {
          gl_FragColor = texture2D(u_texture, v_texCoord);
        } else {
          // Procedural Portuguese flag: green (40% left), red (60% right)
          vec2 uv = v_texCoord;
          
          // Portuguese flag colors (more vibrant)
          vec3 green = vec3(0.0, 0.6, 0.0);  // Brighter green
          vec3 red = vec3(0.85, 0.15, 0.2);   // Brighter red
          
          // Split at 40% - green on left, red on right
          float split = 0.4;
          vec3 flagColor = uv.x < split ? green : red;
          
          // Emblem: circular, centered at 40% x (color boundary), 50% y
          vec2 center = vec2(split, 0.5);
          float dist = distance(uv, center);
          float emblemRadius = 0.15;
          
          if (dist < emblemRadius) {
            // Outer golden ring
            if (dist > emblemRadius * 0.75) {
              vec3 gold = vec3(1.0, 0.85, 0.4);
              flagColor = mix(flagColor, gold, 0.9);
            } else if (dist > emblemRadius * 0.5) {
              // Middle white ring
              vec3 white = vec3(0.98, 0.98, 0.98);
              flagColor = mix(flagColor, white, 0.85);
            } else {
              // Inner blue/white center
              vec3 centerColor = vec3(0.2, 0.3, 0.6);
              flagColor = mix(flagColor, centerColor, 0.7);
            }
          }
          
          gl_FragColor = vec4(flagColor, 1.0);
        }
      }
    `;

    function createShader(gl: WebGLRenderingContext, type: number, source: string) {
      const shader = gl.createShader(type);
      if (!shader) return null;
      gl.shaderSource(shader, source);
      gl.compileShader(shader);
      if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
        console.error("Shader compile error:", gl.getShaderInfoLog(shader));
        gl.deleteShader(shader);
        return null;
      }
      return shader;
    }

    function createProgram(gl: WebGLRenderingContext, vs: WebGLShader, fs: WebGLShader) {
      const program = gl.createProgram();
      if (!program) return null;
      gl.attachShader(program, vs);
      gl.attachShader(program, fs);
      gl.linkProgram(program);
      if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
        console.error("Program link error:", gl.getProgramInfoLog(program));
        gl.deleteProgram(program);
        return null;
      }
      return program;
    }

    const vs = createShader(gl, gl.VERTEX_SHADER, vertexSrc);
    const fs = createShader(gl, gl.FRAGMENT_SHADER, fragmentSrc);
    if (!vs || !fs) return;
    const program = createProgram(gl, vs, fs);
    if (!program) return;

    gl.useProgram(program);

    const a_position = gl.getAttribLocation(program, "a_position");
    const a_texCoord = gl.getAttribLocation(program, "a_texCoord");
    const u_time = gl.getUniformLocation(program, "u_time");
    const u_texture = gl.getUniformLocation(program, "u_texture");
    const u_useTexture = gl.getUniformLocation(program, "u_useTexture");
    
    let textureLoaded = false;

    // ----------- GEOMETRY: subdivided quad in clipspace [-1,1] -----------
    const segmentsX = 40;
    const segmentsY = 24;

    const positions: number[] = [];
    const texCoords: number[] = [];
    const indices: number[] = [];

    for (let y = 0; y <= segmentsY; y++) {
      const v = y / segmentsY;
      const py = -1 + v * 2; // -1 to +1
      for (let x = 0; x <= segmentsX; x++) {
        const u = x / segmentsX;
        const px = -1 + u * 2; // -1 to +1

        positions.push(px, py, 0);
        // Flip vertically only (top to bottom)
        texCoords.push(u, 1 - v);
      }
    }

    for (let y = 0; y < segmentsY; y++) {
      for (let x = 0; x < segmentsX; x++) {
        const i = y * (segmentsX + 1) + x;
        const iRight = i + 1;
        const iDown = i + (segmentsX + 1);
        const iDownRight = iDown + 1;

        indices.push(i, iRight, iDown);
        indices.push(iRight, iDownRight, iDown);
      }
    }

    function makeBuffer(
      target: number,
      data: BufferSource,
      usage: number = gl.STATIC_DRAW
    ): WebGLBuffer | null {
      const buffer = gl.createBuffer();
      if (!buffer) return null;
      gl.bindBuffer(target, buffer);
      gl.bufferData(target, data, usage);
      return buffer;
    }

    const positionBuffer = makeBuffer(gl.ARRAY_BUFFER, new Float32Array(positions));
    const texCoordBuffer = makeBuffer(gl.ARRAY_BUFFER, new Float32Array(texCoords));
    const indexBuffer = makeBuffer(gl.ELEMENT_ARRAY_BUFFER, new Uint16Array(indices));

    if (!positionBuffer || !texCoordBuffer || !indexBuffer) {
      console.error("Failed to create buffers");
      return;
    }

    // ----------- TEXTURE SETUP -----------
    const texture = gl.createTexture();
    if (!texture) {
      console.error("Failed to create texture");
      return;
    }

    gl.bindTexture(gl.TEXTURE_2D, texture);
    // temporary 1x1 green pixel while image loads
    gl.texImage2D(
      gl.TEXTURE_2D,
      0,
      gl.RGBA,
      1,
      1,
      0,
      gl.RGBA,
      gl.UNSIGNED_BYTE,
      new Uint8Array([0, 102, 0, 255])
    );

    const image = new Image();
    image.src = "/flags/pt-real.png";
    image.onload = () => {
      gl.bindTexture(gl.TEXTURE_2D, texture);
      gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, image);

      // choose filter based on power-of-two
      const isPOT =
        (image.width & (image.width - 1)) === 0 &&
        (image.height & (image.height - 1)) === 0;

      if (isPOT) {
        gl.generateMipmap(gl.TEXTURE_2D);
      } else {
        gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
        gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);
        gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
      }
      
      textureLoaded = true;
    };
    image.onerror = () => {
      // Silently fall back to procedural rendering
      textureLoaded = false;
    };

    // ----------- RENDER LOOP -----------
    gl.enable(gl.DEPTH_TEST);
    gl.clearColor(0.05, 0.05, 0.08, 1.0);

    let frameId: number;

    const render = (timeMs: number) => {
      const time = timeMs * 0.001;

      // Resize canvas to CSS size
      const displayWidth = canvas.clientWidth || canvas.offsetWidth || 800;
      const displayHeight = canvas.clientHeight || canvas.offsetHeight || 450;
      if (canvas.width !== displayWidth || canvas.height !== displayHeight) {
        canvas.width = displayWidth;
        canvas.height = displayHeight;
        gl.viewport(0, 0, canvas.width, canvas.height);
      }

      gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

      gl.useProgram(program);

      // positions
      gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
      gl.enableVertexAttribArray(a_position);
      gl.vertexAttribPointer(a_position, 3, gl.FLOAT, false, 0, 0);

      // texCoords
      gl.bindBuffer(gl.ARRAY_BUFFER, texCoordBuffer);
      gl.enableVertexAttribArray(a_texCoord);
      gl.vertexAttribPointer(a_texCoord, 2, gl.FLOAT, false, 0, 0);

      // indices
      gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, indexBuffer);

      gl.uniform1f(u_time, time);
      gl.uniform1i(u_texture, 0);
      gl.uniform1i(u_useTexture, textureLoaded ? 1 : 0);

      gl.drawElements(gl.TRIANGLES, indices.length, gl.UNSIGNED_SHORT, 0);

      frameId = requestAnimationFrame(render);
    };

    frameId = requestAnimationFrame(render);

    return () => {
      if (frameId) cancelAnimationFrame(frameId);
    };
  }, []);

  return (
    <div className="flex h-full w-full items-center justify-center bg-slate-950">
      <canvas ref={canvasRef} className="h-full w-full" />
    </div>
  );
};

export default PortugueseFlagWebGL;

