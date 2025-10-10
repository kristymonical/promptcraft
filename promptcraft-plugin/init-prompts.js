// Init Prompts - Figma plugin command script
// Loads data from ux-prompts-full.json and stores under 'prompts' in figma.clientStorage

// Data from ux-prompts-full.json (50 prompts: 10 per category)
const UX_PROMPTS_DATA = [
  // Prototype Review & UX Feedback (8 prompts)
  {"id":"protoai-001","title":"Prototype Usability Audit","description":"Evaluate a Figma prototype for usability, clarity, and task flow efficiency.","category":"Prototype Review & UX Feedback","tags":["#prototypeReview","#usability","#feedback"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-002","title":"Interaction Flow Evaluation","description":"Analyze user interaction flow and identify unnecessary steps or friction points.","category":"Prototype Review & UX Feedback","tags":["#interaction","#uxReview"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-003","title":"Visual Hierarchy Feedback","description":"Evaluate hierarchy and readability of components within a prototype.","category":"Prototype Review & UX Feedback","tags":["#visualDesign","#hierarchy"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-004","title":"Cognitive Load Assessment","description":"Detect information overload or decision fatigue in a complex flow.","category":"Prototype Review & UX Feedback","tags":["#uxResearch","#cognitiveLoad"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-005","title":"Micro-Interaction Review","description":"Review animations and transitions for purpose, speed, and consistency.","category":"Prototype Review & UX Feedback","tags":["#motion","#interaction"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-006","title":"Onboarding Clarity Check","description":"Identify potential confusion points in onboarding or first-time use flows.","category":"Prototype Review & UX Feedback","tags":["#onboarding","#usability"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-007","title":"Feedback Clarity Review","description":"Ensure feedback messages clearly communicate system responses.","category":"Prototype Review & UX Feedback","tags":["#feedback","#uxwriting"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-008","title":"Error State Review","description":"Analyze how error states communicate cause and recovery paths.","category":"Prototype Review & UX Feedback","tags":["#error","#usability"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},

  // Collaboration & Handoff (4 prompts)
  {"id":"protoai-020","title":"Developer Readiness Audit","description":"Identify missing context, unclear logic, or handoff blockers for FE devs.","category":"Collaboration & Handoff","tags":["#handoff","#frontend","#communication"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-021","title":"Design Intent Summary","description":"Summarize the purpose and intent of the design for developers.","category":"Collaboration & Handoff","tags":["#handoff","#intent"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-022","title":"State Documentation Generator","description":"Document all interaction states in a developer-friendly summary.","category":"Collaboration & Handoff","tags":["#documentation","#handoff"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-023","title":"Design-Dev Alignment Report","description":"Generate a summary of design decisions that affect technical feasibility.","category":"Collaboration & Handoff","tags":["#alignment","#handoff"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},

  // Accessibility & QA (4 prompts)
  {"id":"protoai-040","title":"Accessibility Compliance Audit","description":"Run an accessibility audit on color, contrast, and keyboard focus.","category":"Accessibility & QA","tags":["#a11y","#wcag","#contrast"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-041","title":"Keyboard Navigation Review","description":"Assess logical focus order and keyboard accessibility.","category":"Accessibility & QA","tags":["#keyboard","#accessibility"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-042","title":"Touch Target Validation","description":"Check minimum tappable areas for mobile and touch devices.","category":"Accessibility & QA","tags":["#touch","#mobile"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-043","title":"Inclusive Design Review","description":"Identify assumptions that might exclude users with differing abilities.","category":"Accessibility & QA","tags":["#inclusiveDesign","#a11y"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},

  // Experimentation & Iteration (3 prompts)
  {"id":"protoai-060","title":"Usability Hypothesis Generator","description":"Generate hypotheses to validate through testing based on this prototype.","category":"Experimentation & Iteration","tags":["#testing","#research"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-061","title":"Variant Exploration Prompt","description":"Suggest small layout or copy variations for A/B testing.","category":"Experimentation & Iteration","tags":["#abTesting","#iteration"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-062","title":"Experiment Setup Plan","description":"Outline what metrics and goals to track for usability testing.","category":"Experimentation & Iteration","tags":["#experiment","#metrics"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},

  // Design Ops & Governance (3 prompts)
  {"id":"protoai-080","title":"Design Token Audit","description":"Identify inconsistent or duplicated design tokens in this frame.","category":"Design Ops & Governance","tags":["#tokens","#designOps"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-081","title":"Auto-Layout QA","description":"Ensure all auto-layout components follow consistent spacing logic.","category":"Design Ops & Governance","tags":["#autoLayout","#qa"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"},
  {"id":"protoai-082","title":"Naming Consistency Check","description":"Ensure frame, layer, and component naming follows standards.","category":"Design Ops & Governance","tags":["#governance","#consistency"],"model":"Claude","createdAt":"2025-10-10T00:00:00Z"}
];

// Store prompts in figma.clientStorage
figma.clientStorage.setAsync('prompts', UX_PROMPTS_DATA).then(function() {
  console.log('Prompts stored:', UX_PROMPTS_DATA.length);
  figma.notify('Prompts loaded successfully!');
  figma.closePlugin();
}).catch(function(error) {
  console.error('Failed to store prompts:', error);
  figma.notify('Failed to load prompts');
  figma.closePlugin();
});