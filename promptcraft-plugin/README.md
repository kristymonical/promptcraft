# PromptCraft - Figma Plugin

A powerful Figma plugin that provides curated UX/UI prompts for AI assistants like Claude and GPT, helping designers work more efficiently.

## Features

- 🎯 **Curated Prompts** - 75+ professional UX prompts across 5 categories
- ⭐ **My Library** - Save your favorite prompts for quick access
- 🔍 **Smart Search** - Filter prompts by title, description, or tags
- 📋 **Copy to Clipboard** - One-click copy for any prompt
- ✏️ **Custom Prompts** - Create and edit your own prompts
- 🎨 **Modern UI** - Clean, intuitive interface with Material Icons

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
├── code.js                 # Backend plugin logic
├── ui.html                 # Frontend UI (includes modular architecture)
├── data/
│   └── ux-prompts-full.json  # Prompt data (75 curated prompts)
├── package.json            # Project dependencies
└── README.md              # This file
```

## Installation

1. Clone this repository
2. Open Figma Desktop App
3. Go to Plugins → Development → Import plugin from manifest
4. Select the `manifest.json` file
5. The plugin is now ready to use!

## Usage

### Basic Operations
1. **Browse Prompts** - Click category tabs to filter by topic
2. **Search** - Type in the search box to find specific prompts
3. **Favorite** - Click the star icon to save prompts to My Library
4. **Copy** - Select a prompt and click "Copy Prompt" to copy to clipboard
5. **Create** - Click "Create Prompt" to add your own custom prompts
6. **Edit** - Click "Edit" on any custom prompt to modify it

### Creating Custom Prompts
1. Click the "Create Prompt" button
2. Choose between:
   - **Manual** - Fill in title, description, and category
   - **AI-Assisted** - Describe what you need and let AI generate a prompt (mock for now)
3. Click "Save Prompt" to add to your library

### Managing Prompts
- **Edit**: Click "Edit" on any custom prompt card
- **Delete**: Click "Delete Prompt" in the edit form
- **Favorite**: Star any prompt to add it to My Library

## Architecture

### Modular Design
The plugin uses a modular architecture with embedded modules for maintainability:

- **SearchModule** - Handles search and filtering logic
- **CategoriesModule** - Manages category tabs and switching
- **FavoritesModule** - Handles favorite prompts management
- **LazyLoadModule** - Manages infinite scrolling

All modules are embedded in `ui.html` for Figma compatibility.

### Data Storage
- Prompts are stored in `figma.clientStorage` for persistence
- Favorites are synced across sessions
- Custom prompts are saved alongside default prompts

### State Management
- React hooks (useState, useEffect, useRef) for UI state
- Module-based state for search, categories, and favorites
- Single source of truth for each feature

## Development

### Prerequisites
- Node.js (for package management)
- Figma Desktop App

### Setup
```bash
npm install
```

### Code Structure
- `code.js` - ES5 JavaScript for Figma backend
- `ui.html` - Preact-based UI with inline CSS and modules
- `data/ux-prompts-full.json` - Prompt dataset

### Key Technologies
- **Preact** (via CDN) - Lightweight React alternative
- **Material Icons** - Icon system
- **Figma Plugin API** - clientStorage, messaging, UI
- **ES5 JavaScript** - For maximum Figma compatibility

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

## License

MIT License - Feel free to use and modify as needed!

## Support

For issues or questions, please open an issue on GitHub.

---

**Made with ❤️ for the design community**