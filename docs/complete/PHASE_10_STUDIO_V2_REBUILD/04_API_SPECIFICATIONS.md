# GFC Studio - API Specifications

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** REST API & WebSocket Specifications

---

## 🌐 Overview

This document specifies the REST API endpoints and real-time communication protocols used by GFC Studio and the Next.js Website.

**Base URL:** `https://api.gloucesterfraternityclub.com/v1/studio`  
**Authentication:** JWT (Bearer Token)  
**Content-Type:** `application/json`

---

## 🔐 Authentication & Security

### Authorization Header
All requests must include a valid JWT token obtained from the GFC Identity Server.

```http
Authorization: Bearer <jwt_token>
```

### Error Responses
Common error format:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human readable message",
    "details": {}
  }
}
```

- `401 Unauthorized`: Missing or invalid token.
- `403 Forbidden`: User lacks permission for the resource.
- `404 Not Found`: Resource does not exist.
- `422 Unprocessable Entity`: Validation failure.
- `500 Internal Server Error`: Server-side failure.

---

## 📄 Pages API

### 1. List All Pages
`GET /pages`

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "title": "Home",
    "slug": "/",
    "status": "Published",
    "updatedAt": "2024-12-24T10:00:00Z",
    "updatedBy": "admin"
  }
]
```

### 2. Create Page
`POST /pages`

**Request Body:**
```json
{
  "title": "New Page",
  "slug": "/new-page",
  "templateId": 5
}
```

### 3. Get Page Details (for Editor)
`GET /pages/{id}`

- Returns metadata + serialized section data for the studio canvas.

### 4. Update Page Metadata
`PATCH /pages/{id}`

**Request Body:**
```json
{
  "title": "Updated Title",
  "metaTitle": "SEO Title",
  "status": "Draft"
}
```

### 5. Publish Page
`POST /pages/{id}/publish`

- Triggers cache invalidation and Next.js revalidation (ISR).

---

## 🧱 Sections & Components API

### 1. Get Page Sections
`GET /pages/{id}/sections`

**Response:**
```json
[
  {
    "id": 101,
    "type": "Hero",
    "order": 0,
    "content": { "title": "Welcome", ... },
    "styles": { "bg": "#000" }
  }
]
```

### 2. Update Section Order
`PUT /pages/{id}/sections/reorder`

**Request Body:**
```json
{
  "sections": [
    { "id": 101, "order": 0 },
    { "id": 105, "order": 1 }
  ]
}
```

### 3. Add Component/Section
`POST /pages/{id}/sections`

**Request Body:**
```json
{
  "type": "CardGrid",
  "order": 2,
  "content": {}
}
```

---

## 📂 Media API

### 1. Upload Media
`POST /media/upload`

**Form Data:**
- `file`: Binary Data
- `folderId`: Optional

### 2. Browse Media Library
`GET /media`

**Query Params:** `folderId`, `type`, `search`

### 3. Delete Media
`DELETE /media/{id}`

---

## 📝 Forms API

### 1. Save Form Submission
`POST /forms/submit`

- Used by the public website to send data to the Studio backend.

### 2. Get Form Responses
`GET /forms/{id}/submissions`

---

## 🔄 Real-time Preview (WebSockets)

### Hub URL: `/hubs/studio-preview`

**Events:**
- `Subscribe(pageId)`: Joins a room for real-time updates.
- `ContentChanged(data)`: Sent by Studio to the Preview iframe to update UI without reload.

---

## 💡 Implementation Notes
- **Rate Limiting:** 100 requests per minute for non-admin users.
- **Caching:** GET requests use `Cache-Control: private, max-age=0` in Studio to ensure data freshness.

---

**Next:** 05_ANIMATION_SYSTEM.md ➜
