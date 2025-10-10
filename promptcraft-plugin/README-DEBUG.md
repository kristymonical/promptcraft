# Debug Instructions

## Issue: Categories from ux-prompts-full.json not displaying

The plugin is currently using hardcoded fallback data instead of the data from `ux-prompts-full.json`.

## Solution Steps:

### 1. Load the JSON data into Figma clientStorage
1. The manifest.json is currently set to `"main": "init-prompts.js"`
2. Run the plugin in Figma (it will load the JSON data and close automatically)
3. You should see "Prompts loaded successfully!" notification

### 2. Switch back to the main plugin
1. Change manifest.json back to `"main": "code.js"`
2. Run the plugin again - it should now show the correct categories from the JSON file

### 3. Expected Categories:
- **Prototype Review & UX Feedback** (8 prompts)
- **Collaboration & Handoff** (4 prompts) 
- **Accessibility & QA** (4 prompts)
- **Experimentation & Iteration** (3 prompts)
- **Design Ops & Governance** (3 prompts)

### 4. Debug Console Output:
Look for these console messages:
- "Prompts stored: 22" (when running init-prompts.js)
- "Available categories in data: [...]" (when running main plugin)
- "Filtered to X prompts for category: [category name]"

## Files Updated:
- ✅ `init-prompts.js` - Now contains data from ux-prompts-full.json
- ✅ `manifest.json` - Temporarily set to init-prompts.js
- ✅ `code.js` - Has debugging output for category filtering
