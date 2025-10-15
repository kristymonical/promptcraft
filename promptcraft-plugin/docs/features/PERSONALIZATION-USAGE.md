# Personalization Feature - User Guide

## 🎯 Overview

The Figma plugin now includes a personalization feature that lets users customize their experience for Figma Make prototyping workflows.

## 📱 User Journey

### First Time Experience

```
1. User opens plugin for the first time
   ↓
2. "Personalize Your Experience" modal appears automatically
   ↓
3. User sees 9 categories:
   - UX Research
   - Accessibility
   - Prototyping
   - User Testing
   - Ideation
   - Code Generation
   - Image & Asset Creation
   - Interactive Prototyping
   - Advanced Iteration
   ↓
4. User clicks a category to expand it
   ↓
5. User sees 8-10 specific prompts (e.g., "Generate a WCAG-compliant navbar with Figma Make's Gemini AI")
   ↓
6. User checks categories/prompts they want
   ↓
7. User clicks "Save" or "Skip"
   ↓
8. Modal closes, green toast appears: "Nav settings saved"
```

### Customizing Later

```
1. User clicks "Customize Nav" button in nav bar
   ↓
2. Modal reopens with previous selections
   ↓
3. Tooltip appears (first time only):
   "Tailor your nav to your role—hide categories or prompts you don't need!"
   ↓
4. User modifies selections
   ↓
5. User clicks "Save"
   ↓
6. Settings update, success toast shows
```

## 🎨 UI Elements

### Modal Structure
```
┌─────────────────────────────────────┐
│  Personalize Your Experience        │  ← Header (16px, bold)
├─────────────────────────────────────┤
│  Select categories for your Figma   │
│  Make prototyping workflow          │  ← Description
│                                     │
│  ┌─ [ ] UX Research            ▶  │  ← Category (collapsed)
│  │                                 │
│  ┌─ [✓] Accessibility          ▼  │  ← Category (expanded)
│  │   [✓] Generate WCAG-compliant... │
│  │   [✓] Create accessible colors   │
│  │   [ ] Generate ARIA labels       │
│  │   [ ] Check contrast ratios      │
│  │   ...                            │
│  │                                  │
│  ┌─ [ ] Prototyping            ▶  │
│  │                                  │
│  ... (7 more categories)            │
├─────────────────────────────────────┤
│                   [Skip]  [Save]    │  ← Buttons
└─────────────────────────────────────┘
```

### Nav Bar with Customize Button
```
┌─────────────────────────────────────────────────────────┐
│  [All] [My Library] [Category1] [Category2]... [Customize Nav]  │
│  ↑ Existing tabs                                ↑ New button    │
└─────────────────────────────────────────────────────────┘
             ▲
             └─ Tooltip appears here on first click:
                "Tailor your nav to your role..."
```

## 🎭 Example Use Cases

### UX Researcher
**Selects:**
- ✓ UX Research (all prompts)
- ✓ User Testing (all prompts)
- ✓ Accessibility (selected prompts)
- ✗ Code Generation
- ✗ Interactive Prototyping

**Result:** Clean nav bar with only relevant categories

### Frontend Developer
**Selects:**
- ✓ Code Generation (all prompts)
- ✓ Interactive Prototyping (all prompts)
- ✓ Accessibility (ARIA labels, keyboard nav)
- ✗ UX Research
- ✗ Ideation

**Result:** Developer-focused prompt library

### Full-Stack Designer
**Selects:**
- ✓ All categories
- ✗ Some advanced prompts in each

**Result:** Broad but curated toolset

## 🔧 Technical Details

### Data Sent to Backend
```javascript
{
    type: 'save-nav-settings',
    payload: {
        categories: [
            'UX Research',
            'Accessibility',
            'Code Generation'
        ],
        prompts: {
            'UX Research': [
                'Generate a user research plan for Figma Make',
                'Create interview questions for UX testing'
            ],
            'Accessibility': [
                'Generate a WCAG-compliant navbar with Figma Make\'s Gemini AI',
                'Create accessible color palettes'
            ],
            'Code Generation': [
                'Generate React components from designs',
                'Create Tailwind CSS classes'
            ]
        }
    }
}
```

### Console Logs
```
"Personalize modal opened"        ← First load
"Customize nav clicked"           ← Button clicked
"Nav settings saved"              ← Settings saved
```

### localStorage Key
```
hasSeenPersonalization: 'true'
```

## 💡 Pro Tips

### For Users:
1. **Start Broad**: Check all categories on first load, refine later
2. **Role-Based**: Tailor to your primary role (designer, developer, researcher)
3. **Quick Access**: Use "Customize Nav" to adjust as projects change
4. **Tooltip Info**: Read the tooltip for customization tips

### For Developers:
1. **Filtering**: Use saved settings to filter visible tabs/prompts
2. **Analytics**: Track which categories/prompts are most popular
3. **Sync**: Consider syncing settings across devices
4. **Defaults**: Provide smart defaults based on user role

## 🎯 Future Enhancements (Ideas)

- **Presets**: "UX Researcher", "Frontend Dev", "Full-Stack Designer"
- **Import/Export**: Share settings with team
- **Smart Suggestions**: "Based on your usage, you might like..."
- **Role Detection**: Auto-suggest categories based on Figma file type
- **Usage Analytics**: Show most-used prompts
- **Quick Toggle**: Right-click tab to hide/show
- **Search**: Filter categories/prompts in modal
- **Recent**: Auto-show recently used prompts

## 📊 Success Metrics

Track these to measure adoption:
- **Modal Completion Rate**: % who click Save vs Skip
- **Average Categories Selected**: 3-5 is ideal
- **Customization Frequency**: How often users re-open modal
- **Tooltip Engagement**: Helps understand if users need guidance
- **Category Popularity**: Which categories are selected most

## ✅ Ready to Use!

The personalization feature is fully functional and ready for users. It enhances the Figma Make prototyping workflow by allowing users to focus on what matters most to their role.
