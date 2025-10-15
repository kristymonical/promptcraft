# Personalization Feature - Debug Guide

## 🐛 Issues Reported
1. Modal not showing on load
2. Prompts not appearing in modal
3. Filtering not working after save

## ✅ Fixes Applied

### 1. **Modal Always Shows (Testing Mode)**
Changed the modal to show on every load for testing:
```javascript
// TEMP: Always show for testing
setShowPersonalizeModal(true);
```

**To re-enable localStorage check** (after testing):
- Uncomment lines 1474-1479 in ui.html
- Comment out lines 1470-1471

### 2. **Added Extensive Debug Logging**

#### Console Logs Added:
- `"Component render - showPersonalizeModal: true/false"` - Every render
- `"Component render - navSettings: {...}"` - Shows saved settings
- `"Personalize modal opened"` - When modal opens
- `"Customize nav clicked"` - When customize button clicked
- `"Nav settings saved: {...}"` - Full settings object
- `"Selected categories: [...]"` - Array of selected categories
- `"Selected prompts: {...}"` - Object of selected prompts per category
- `"Nav settings changed: {...}"` - When navSettings state updates
- `"Visible categories after settings change: [...]"` - Filtered category list
- `"Visible categories: [...]"` - Every time getVisibleCategories() is called

### 3. **Category Filtering Logic**

Added `getVisibleCategories()` function that:
- Always shows 'All' and 'My Library'
- Maps personalization categories to existing plugin categories
- Returns filtered list based on saved settings

**Category Mapping:**
```javascript
'UX Research' → 'Prototype Review & UX Feedback'
'Accessibility' → 'Accessibility & QA'
'Prototyping' → 'Experimentation & Iteration'
'User Testing' → 'Prototype Review & UX Feedback'
'Ideation' → 'Design Ops & Governance'
'Code Generation' → 'Collaboration & Handoff'
'Image & Asset Creation' → 'Design Ops & Governance'
'Interactive Prototyping' → 'Experimentation & Iteration'
'Advanced Iteration' → 'Design Ops & Governance'
```

### 4. **Tab Rendering Update**
Changed from:
```javascript
categories.map(cat => ...)
```

To:
```javascript
getVisibleCategories().map(cat => ...)
```

## 🔍 Testing Steps

### Step 1: Check Modal Appears
1. Open Figma plugin
2. Look for console log: `"Component render - showPersonalizeModal: true"`
3. Modal should appear immediately

**If modal doesn't appear:**
- Check browser console for errors
- Verify `showPersonalizeModal` state is `true`
- Check if modal overlay is rendering (inspect DOM for `#personalize-modal`)

### Step 2: Check Prompts Display
1. Modal should show 9 categories
2. Each category should have a caret icon (▶)
3. Click a category to expand it
4. Should see 8 prompts listed

**If prompts don't appear:**
- Check console for `personalizationData` object
- Verify `Object.keys(personalizationData)` returns 9 categories
- Inspect DOM for `.category-section` elements

### Step 3: Test Selection
1. Check a category checkbox
2. Category should auto-expand
3. All prompts in that category should check
4. Console should log selection changes

**Expected behavior:**
- Checking category → checks all prompts
- Unchecking category → unchecks all prompts
- Can individually check/uncheck prompts

### Step 4: Test Save
1. Select at least one category
2. Click "Save" button
3. Console logs should appear:
   ```
   Nav settings saved: {categories: [...], prompts: {...}}
   Selected categories: [...]
   Selected prompts: {...}
   Nav settings changed: {...}
   Visible categories after settings change: [...]
   ```
4. Green toast "Nav settings saved" should appear
5. Modal should close

### Step 5: Check Tab Filtering
1. After saving, look at nav tabs
2. Should show: All + My Library + selected categories (mapped)
3. Console shows: `"Visible categories: [...]"`

**Example:**
If you select "Accessibility" and "Code Generation":
- Should see: All, My Library, Accessibility & QA, Collaboration & Handoff

### Step 6: Test Customize Button
1. Click "Customize Nav" button
2. Modal should reopen
3. Previously selected categories/prompts should be checked
4. Tooltip should appear (first time only)

## 🐛 Common Issues & Solutions

### Issue: Modal Not Appearing
**Check:**
- Console log shows `showPersonalizeModal: true`
- No JavaScript errors in console
- Modal CSS is present (check dev tools)

**Solution:**
- Refresh plugin
- Check if modal z-index (2000) is high enough
- Verify modal overlay exists

### Issue: Prompts Not Listed
**Check:**
- `personalizationData` object is defined
- `Object.keys(personalizationData).length === 9`
- Categories are expanding (caret rotates)

**Solution:**
- Check `.prompts-list.expanded` CSS
- Verify `expandedCategories` state updates
- Check console for render errors

### Issue: Filtering Not Working
**Check:**
- Console shows "Nav settings changed"
- `navSettings` object has data
- `getVisibleCategories()` returns correct array

**Solution:**
- Verify `navSettings` state is updating
- Check category mapping logic
- Force re-render by toggling state

### Issue: Checkboxes Not Working
**Check:**
- `selectedCategories` state updates
- `selectedPrompts` state updates
- Console logs show selection changes

**Solution:**
- Verify event handlers are attached
- Check stopPropagation on nested clicks
- Ensure state updates trigger re-render

## 📊 Expected Console Output

### On Load:
```
Component render - showPersonalizeModal: true
Component render - navSettings: null
Personalize modal opened
```

### After Selecting Categories:
```
(Multiple renders as user checks boxes)
Component render - showPersonalizeModal: true
Component render - navSettings: null
```

### After Clicking Save:
```
Nav settings saved: {
  categories: ["Accessibility", "Code Generation"],
  prompts: {
    "Accessibility": ["Generate a WCAG-compliant navbar...", ...],
    "Code Generation": ["Generate React components...", ...]
  }
}
Selected categories: ["Accessibility", "Code Generation"]
Selected prompts: {...}
Component render - showPersonalizeModal: false
Nav settings changed: {...}
Visible categories after settings change: ["All", "My Library", "Accessibility & QA", "Collaboration & Handoff"]
Visible categories: ["All", "My Library", "Accessibility & QA", "Collaboration & Handoff"]
```

### After Clicking Customize Nav:
```
Customize nav clicked
Component render - showPersonalizeModal: true
```

## 🎯 Quick Debug Checklist

Run through this in order:

- [ ] Plugin loads without errors
- [ ] Console shows "Component render" logs
- [ ] Modal appears (`showPersonalizeModal: true`)
- [ ] 9 categories visible in modal
- [ ] Categories expand/collapse on click
- [ ] Prompts appear when category expanded
- [ ] Category checkbox checks all prompts
- [ ] Individual prompts can be checked
- [ ] Save button closes modal
- [ ] Console shows "Nav settings saved"
- [ ] Success toast appears
- [ ] Console shows "Visible categories"
- [ ] Tabs in nav bar change
- [ ] Customize Nav button works
- [ ] Modal reopens with selections
- [ ] Tooltip appears on first click

## 🔧 If Still Not Working

1. **Clear localStorage:**
   ```javascript
   localStorage.clear()
   ```

2. **Force modal open:**
   In console:
   ```javascript
   // Find React root and force state update
   // This is a hack for testing
   ```

3. **Check React/Preact version:**
   Ensure Preact 10.5.14 is loading correctly

4. **Inspect DOM:**
   - Look for `#personalize-modal` element
   - Check if `.modal-overlay` exists
   - Verify modal is not hidden by CSS

5. **Check state in React DevTools:**
   If available, inspect component state

## 📝 Next Steps

After debugging:
1. Implement prompt filtering (filter prompts list based on selections)
2. Persist settings to `figma.clientStorage`
3. Add loading states
4. Add validation (must select at least one category)
5. Add "Select All" / "Deselect All" buttons
6. Improve UX with animations
7. Add keyboard shortcuts (Esc to close)
