# FSNotes implementation plan

This plan is saved in the session workspace so it can be reviewed before implementation begins.

## Problem and current state
The repository is effectively empty apart from `Idea.md`, which describes a markdown note-taking and blogging app with a clipboard-to-image feature and a lightweight storage model. There is no existing app scaffold, data layer, editor implementation, or deployment setup. The plan below assumes a greenfield implementation with a local-first experience and flexible storage options.

## Proposed approach
Build a small, local-first markdown notes app that focuses on a clean note editor, flexible storage, and clipboard image embedding. Start with the core user flow: create/update notes, organize them into categories, and embed screenshots from clipboard. Keep the first version simple and modular so storage can be swapped between a Git repo, folder-based files, or a SQLite/Postgres-backed approach without redesigning the app.

## Planned workstreams

### 1. Define the product and technical baseline
- Confirm the intended user flow: create notes, edit markdown, categorize content, and use clipboard screenshots.
- Choose the target platform and stack in the .NET ecosystem: .NET MAUI or .NET MAUI Blazor Hybrid.
- Define the MVP scope and explicitly exclude advanced publishing, collaboration, and multi-user sync for v1.
- Decide the default storage strategy for the first version: local folder + markdown files is the simplest and most realistic initial path.

### 2. Scaffold the app foundation
- Create the app project structure and package/build configuration.
- Add a shell for the main window, sidebar navigation, note list, and editor pane.
- For MAUI Blazor Hybrid, define the split between native host responsibilities (windowing, file/clipboard access) and Blazor UI responsibilities (editor, lists, category views).
- Create the data model for notes and categories with a clean domain layer.
- Establish the file abstraction layer so storage backends can be swapped later.

### 3. Implement note editing and categorization
- Build a markdown editing surface with syntax-friendly layout.
- Add note metadata (title, tags, category, updated date).
- Support note creation, rename, delete, and search.
- Define category hierarchy and navigation in the sidebar.
- Add note list sorting and filtering behavior.

### 4. Add clipboard screenshot embedding
- Detect copied image data from the clipboard.
- Convert clipboard content into a local image asset or generated file.
- Insert a markdown image reference into the current editor position.
- Store the generated asset alongside the note or in a dedicated attachments directory.
- Handle sync and cleanup for removed/unused images.

### 5. Implement storage and persistence
- Start with a local filesystem backend that stores notes as markdown files with a simple metadata manifest.
- Support note-to-file mapping and directory-based organization.
- Add a lightweight persistence layer for categories and note indexes.
- Keep the backend interface abstract so Git repository or database storage can be explored later without affecting the app UI.

### 6. Add polish, validation, and release readiness
- Add basic error handling, unsaved-change warnings, and file recovery behavior.
- Test the editor workflow, note organization, and image embedding through focused end-to-end scenarios.
- Add document/attachment cleaning utilities and fallback handling for non-image clipboard data.
- Prepare a basic production-ready build and path for future extension.

## Key technical decisions
- Keep implementation fully within the .NET ecosystem, with MAUI and MAUI Blazor Hybrid as the target app models.
- Prefer a local-first storage implementation for the first milestone because it is the simplest, most robust, and aligns with the idea.
- Keep note content in markdown and attach images as files rather than inline base64 blobs.
- Explicitly separate storage, model, and UI layers to reduce coupling and make future backend changes easy.
- Treat clipboard screenshot support as a first-class feature, not a sidecar feature, with a clear conversion and insertion workflow.

## Notes and considerations
- The “blog posts” idea suggests a future export/publishing capability, but v1 should focus on authoring and organization rather than publishing.
- The app should support a preview state for markdown rendering without requiring a full blog engine.
- Categories are a structural requirement, so they should be stable enough to support later tag-based views and filtering.
- If a database-backed backend is desired later, it can be introduced behind the storage interface without changing editor logic.
