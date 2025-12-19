export const motion = {
  durations: {
    fast: 120,
    normal: 180,
    slow: 260,
  },
  easing: {
    standard: "cubic-bezier(0.2, 0.0, 0.2, 1)",
    decel: "cubic-bezier(0.0, 0.0, 0.2, 1)",
    accel: "cubic-bezier(0.4, 0.0, 1, 1)",
  },
} as const;

const formatProps = (props?: string[]) => (props && props.length ? props.join(", ") : "all");

export const transitionFast = (props?: string[]) =>
  `${formatProps(props)} ${motion.durations.fast}ms ${motion.easing.standard}`;

export const transitionSoftScale = () =>
  `transform ${motion.durations.normal}ms ${motion.easing.decel}, opacity ${motion.durations.normal}ms ${motion.easing.standard}`;

export default motion;

