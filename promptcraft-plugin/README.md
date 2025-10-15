# PromptCraft - Figma Plugin

A powerful Figma plugin that provides curated UX/UI prompts for AI assistants like Claude and GPT, helping designers work more efficiently with personalized workflows.

## Features

- 🎯 **Curated Prompts** - 90+ professional UX prompts across 6 categories
- 🎨 **Personalized Navigation** - Customize visible categories based on your workflow
- ⭐ **My Library** - Save your favorite prompts for quick access
- 🔍 **Smart Search** - Real-time filter by title, description, or tags
- 📋 **Copy to Clipboard** - One-click copy with auto-deselect
- ✏️ **Custom Prompts** - Create, edit, and delete your own prompts
- 🚀 **Real-time Updates** - Instant feedback as you type
- 💾 **Persistent Storage** - All settings and favorites saved across sessions
- 🎭 **Modern UI** - Clean, intuitive interface with Material Icons

## Categories

- **All** - Browse all available prompts
- **My Library** - Your starred favorites
- **Prototype Review & UX Feedback** - Get feedback on designs and prototypes
- **Collaboration & Handoff** - Streamline team workflows
- **Accessibility & QA** - Ensure inclusive, quality designs
- **Experimentation & Iteration** - Explore new ideas and variations
- **Design Ops & Governance** - Maintain design systems and standards

## File Structure

```
promptcraft-plugin/
├── manifest.json           # Figma plugin configuration
├── code.js                 # Backend plugin logic with storage
├── ui.html                 # Frontend UI (Preact + modular architecture)
├── data/
│   └── ux-prompts-full.json  # Prompt data (90 curated prompts)
├── docs/                   # Documentation folder
│   ├── features/           # Feature documentation
│   ├── development/        # Development guides
│   └── internal/           # Internal audits & summaries
├── package.json            # Project dependencies
└── README.md              # Main documentation
```

## Installation

1. Clone this repository
2. Open Figma Desktop App
3. Go to Plugins → Development → Import plugin from manifest
4. Select the `manifest.json` file
5. The plugin is now ready to use!

## Usage

### First Time Setup (Onboarding)
On first launch, you'll see a personalization modal:
1. Select categories relevant to your workflow (e.g., Prototyping, Accessibility, UX Research)
2. Click "Save" to customize your navigation tabs
3. Click "Skip" to use all default categories
4. Access "Customize Nav" from settings (⚙️) anytime to change preferences

### Basic Operations
1. **Browse Prompts** - Click category tabs to filter by topic
2. **Search** - Type in the search box for real-time filtering
3. **Favorite** - Click the star icon to save prompts to My Library
4. **Copy** - Select a prompt and click "Copy Prompt" (auto-deselects after copying)
5. **Create** - Click "Create Prompt" to add custom prompts
6. **Edit** - Click "Edit" on any prompt to modify it

### Creating Custom Prompts
1. Click the "Create Prompt" button
2. Choose between:
   - **Manual** - Fill in title, description, and category (button activates as you type)
   - **AI-Assisted** - Describe what you need and let AI generate a prompt (mock for now)
3. Click "Create Prompt" when the button becomes active (green)
4. Click "Cancel" to discard

### Managing Prompts
- **Edit**: Click "Edit" on any prompt card
  - "Save Changes" button only activates when you make changes
  - Changes apply in real-time as you type
- **Delete**: Click "Delete Prompt" in edit form (custom prompts only)
- **Favorite**: Star any prompt to add/remove from My Library

### Customizing Navigation
1. Click settings icon (⚙️) in top-right
2. Select "Customize Nav"
3. Check/uncheck categories to show/hide
4. Click "Save" to apply or "Cancel" to discard changes
5. Your preferences persist across sessions

## Architecture

### Modular Design
The plugin uses a modular architecture with embedded modules for maintainability:

- **SearchModule** - Handles search and filtering logic
- **CategoriesModule** - Manages category tabs and switching
- **FavoritesModule** - Handles favorite prompts management
- **LazyLoadModule** - Manages infinite scrolling

All modules are embedded in `ui.html` for Figma compatibility.

### Data Storage
All data persists in `figma.clientStorage`:
- **prompts** - Custom and default prompts
- **favorites** - Starred prompt IDs
- **nav-settings** - Personalized category preferences
  - Includes selected categories and prompt visibility
  - Auto-applies on plugin reload
  - Triggers onboarding modal if not set

### State Management
- **Preact Hooks** (useState, useEffect, useRef) for UI state
- **Module-based state** for search, categories, and favorites
- **Real-time updates** using `onInput` events (Preact compatibility)
- **Persistent settings** synced between backend and UI
- **Single source of truth** for each feature

### Key Improvements
- ✅ Real-time input validation and button activation
- ✅ Proper state management with immutable updates (spread operator)
- ✅ Copy prompt auto-deselects for clear UX
- ✅ Edit form only enables save when changes detected
- ✅ Dynamic Skip/Cancel button based on context
- ✅ Settings persistence across sessions

## Development

### Prerequisites
- Node.js (for package management)
- Figma Desktop App

### Setup
```bash
npm install
```

### Code Structure
- `code.js` - ES6 backend with storage handlers
  - Prompt CRUD operations
  - Settings persistence (nav-settings, favorites)
  - Onboarding flow management
- `ui.html` - Preact-based UI with inline CSS and modules
  - Real-time input handling with `onInput`
  - Modular architecture (Search, Categories, Favorites, LazyLoad)
  - Personalization modal with state management
- `data/ux-prompts-full.json` - Curated prompt dataset (90 prompts)

### Key Technologies
- **Preact** (via CDN) - Lightweight React alternative
- **Material Icons** - Icon system
- **Figma Plugin API** - clientStorage, messaging, notifications, UI
- **ES6 JavaScript** - Modern syntax for backend
- **Immutable State Patterns** - Proper React state management

## Contributing

Feel free to submit issues or pull requests to improve the plugin!

### Adding New Prompts
1. Edit `data/ux-prompts-full.json`
2. Follow the existing format:
   ```json
   {
     "id": "unique-id",
     "title": "Prompt Title",
     "description": "Detailed prompt description",
     "category": "Category Name",
     "tags": ["#detailed"],
     "model": "Claude"
   }
   ```
3. Run the plugin and test

## Recent Updates

### v2.0 - Personalization & UX Enhancements
- 🎨 **Onboarding Flow** - Personalize categories on first launch
- 🚀 **Real-time Inputs** - Instant feedback with `onInput` events
- ⭐ **Fixed Favorites** - Proper state immutability for reliable toggles
- 📋 **Smart Copy** - Auto-deselect after copying prompts
- ✏️ **Smart Editing** - Save button only activates on actual changes
- 🔄 **Dynamic Buttons** - Skip/Cancel context-aware behavior
- 💾 **Settings Persistence** - Nav preferences saved across sessions
- 🎯 **90+ Prompts** - Expanded library across 6 categories

### v1.0 - Initial Release
- Core prompt library with 75+ curated prompts
- Category filtering and search
- Favorites system
- Custom prompt creation
- Lazy loading implementation

## License

MIT License - Feel free to use and modify as needed!

## Documentation

Additional documentation is available in the [`/docs`](./docs) folder:
- **Features** - Implementation guides for specific features
- **Development** - Development and debugging guides  
- **Internal** - Audit reports and improvement summaries

## Support

For issues or questions, please open an issue on GitHub.

## Roadmap

- [ ] AI-powered prompt generation (backend integration)
- [ ] Export/import custom prompts
- [ ] Prompt templates library
- [ ] Team collaboration features
- [ ] Analytics and usage insights

---

**Made with ❤️ for the design community**