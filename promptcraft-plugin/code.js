// PromptCraft Figma Plugin - Backend Code
// Loads prompts from figma.clientStorage with category filtering

// Fallback dataset (75 prompts: 15 per category) used only if clientStorage is empty
const HARD_CODED_FALLBACK = [
  // UX Research (15 prompts)
  { id: 'uxr-001', title: 'User Persona Builder', description: 'Generate a comprehensive user persona for a fintech app, export as JSON for React handoff.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-002', title: 'Journey Mapping Workshop', description: 'Create a user journey map for e-commerce checkout flow with pain points and opportunities.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-003', title: 'Competitive Analysis Framework', description: 'Analyze 3 direct competitors and create feature comparison matrix with insights.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-004', title: 'User Interview Guide', description: 'Design 45-minute user interview script with 8 open-ended questions and follow-ups.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-005', title: 'Survey Design Template', description: 'Create user satisfaction survey with 12 questions and demographic segmentation.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-006', title: 'Card Sorting Analysis', description: 'Design and analyze card sorting study for information architecture optimization.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-007', title: 'A/B Test Hypothesis', description: 'Formulate A/B test hypothesis for homepage conversion rate improvement.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-008', title: 'Stakeholder Interview Plan', description: 'Plan stakeholder interviews to understand business requirements and constraints.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-009', title: 'User Story Mapping', description: 'Create user story map for mobile app onboarding with epics and user stories.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-010', title: 'Mental Model Analysis', description: 'Analyze user mental models for complex dashboard interface design.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-011', title: 'Contextual Inquiry Guide', description: 'Design contextual inquiry study to observe users in their natural environment.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-012', title: 'Focus Group Protocol', description: 'Create focus group discussion guide for product concept validation.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-013', title: 'Heuristic Evaluation Checklist', description: 'Conduct heuristic evaluation using Nielsen\'s 10 principles for usability issues.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-014', title: 'Task Analysis Framework', description: 'Break down complex user tasks into subtasks with time estimates and dependencies.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },
  { id: 'uxr-015', title: 'Research Synthesis Report', description: 'Synthesize user research findings into actionable insights and recommendations.', category: 'UX Research', tags: ['#detailed'], model: 'Claude' },

  // Accessibility (15 prompts)
  { id: 'acc-001', title: 'WCAG AA Compliance Audit', description: 'Conduct WCAG 2.1 AA audit for a React component library with remediation plan.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-002', title: 'Screen Reader Testing', description: 'Test interface with screen readers and document navigation patterns.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-003', title: 'Color Contrast Analysis', description: 'Analyze color contrast ratios and provide accessible color palette alternatives.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-004', title: 'Keyboard Navigation Audit', description: 'Audit keyboard navigation flow and ensure all interactive elements are accessible.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-005', title: 'ARIA Implementation Guide', description: 'Implement proper ARIA labels, roles, and properties for complex UI components.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-006', title: 'Focus Management Strategy', description: 'Design focus management for single-page applications and modal dialogs.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-007', title: 'Alternative Text Guidelines', description: 'Create guidelines for writing effective alt text for images and icons.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-008', title: 'Accessible Form Design', description: 'Design accessible forms with proper labels, error handling, and validation.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-009', title: 'Motion Sensitivity Options', description: 'Implement reduced motion preferences and animation alternatives.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-010', title: 'Voice Control Optimization', description: 'Optimize interface for voice control and speech recognition software.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-011', title: 'Cognitive Load Assessment', description: 'Assess cognitive load and design for users with cognitive disabilities.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-012', title: 'Mobile Accessibility Testing', description: 'Test mobile accessibility features including touch targets and gestures.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-013', title: 'Accessibility Documentation', description: 'Create comprehensive accessibility documentation for development teams.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-014', title: 'Inclusive Design Principles', description: 'Apply inclusive design principles to create products for diverse user needs.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },
  { id: 'acc-015', title: 'Accessibility Testing Protocol', description: 'Develop systematic accessibility testing protocol with automated and manual checks.', category: 'Accessibility', tags: ['#detailed'], model: 'Claude' },

  // Prototyping (15 prompts)
  { id: 'proto-001', title: 'Interactive Prototype Export', description: 'Create high-fidelity Figma prototype with micro-interactions and animations.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-002', title: 'Mobile App Wireframes', description: 'Design mobile app wireframes with user flow and navigation patterns.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-003', title: 'Responsive Design System', description: 'Create responsive design system with breakpoints and component variations.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-004', title: 'Micro-interaction Design', description: 'Design micro-interactions for buttons, forms, and navigation elements.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-005', title: 'Component Library Setup', description: 'Set up Figma component library with variants and auto-layout properties.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-006', title: 'User Flow Diagram', description: 'Create comprehensive user flow diagram with decision points and edge cases.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-007', title: 'Information Architecture', description: 'Design information architecture with sitemap and content hierarchy.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-008', title: 'Dashboard Layout Design', description: 'Design data dashboard with charts, widgets, and responsive grid system.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-009', title: 'Onboarding Flow Prototype', description: 'Create interactive onboarding flow with progressive disclosure and tutorials.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-010', title: 'E-commerce Checkout Flow', description: 'Design streamlined checkout process with payment integration and error handling.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-011', title: 'Mobile Navigation Patterns', description: 'Design mobile navigation patterns including bottom tabs, drawer, and gestures.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-012', title: 'Form Design System', description: 'Create comprehensive form design system with validation states and error handling.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-013', title: 'Loading State Design', description: 'Design loading states, skeletons, and progress indicators for better UX.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-014', title: 'Error State Design', description: 'Design error states, empty states, and 404 pages with helpful messaging.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },
  { id: 'proto-015', title: 'Animation Prototype', description: 'Create animation prototype with easing curves and timing specifications.', category: 'Prototyping', tags: ['#detailed'], model: 'Claude' },

  // User Testing (15 prompts)
  { id: 'test-001', title: 'Usability Test Script', description: 'Write comprehensive usability test script with 5 task scenarios and metrics.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-002', title: 'A/B Test Design', description: 'Design A/B test for homepage conversion with statistical significance requirements.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-003', title: 'Moderated Testing Protocol', description: 'Create moderated user testing protocol with observation guidelines and note-taking.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-004', title: 'Unmoderated Testing Setup', description: 'Set up unmoderated user testing with task completion and satisfaction metrics.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-005', title: 'Card Sorting Analysis', description: 'Analyze card sorting results and create optimal information architecture.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-006', title: 'First Click Testing', description: 'Design first-click test to validate navigation and information hierarchy.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-007', title: 'Tree Testing Protocol', description: 'Create tree testing study to evaluate findability and navigation structure.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-008', title: 'Task Success Metrics', description: 'Define task success metrics and completion criteria for usability testing.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-009', title: 'User Feedback Analysis', description: 'Analyze user feedback and categorize insights for design improvements.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-010', title: 'Remote Testing Setup', description: 'Set up remote user testing with screen recording and analytics integration.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-011', title: 'Accessibility Testing Plan', description: 'Create accessibility testing plan with assistive technology validation.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-012', title: 'Mobile Testing Protocol', description: 'Design mobile user testing protocol with device-specific considerations.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-013', title: 'Cross-browser Testing', description: 'Plan cross-browser testing strategy with compatibility and performance checks.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-014', title: 'Performance Testing', description: 'Design performance testing with load times, responsiveness, and user experience metrics.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },
  { id: 'test-015', title: 'Testing Report Template', description: 'Create comprehensive testing report template with findings and recommendations.', category: 'User Testing', tags: ['#detailed'], model: 'Claude' },

  // Ideation (15 prompts)
  { id: 'idea-001', title: 'Design Sprint Kickoff', description: 'Facilitate 5-day design sprint with problem definition and solution ideation.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-002', title: 'Brainstorming Workshop', description: 'Lead creative brainstorming session with ideation techniques and voting.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-003', title: 'How Might We Questions', description: 'Generate How Might We questions to reframe problems and spark innovation.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-004', title: 'Crazy 8s Exercise', description: 'Facilitate Crazy 8s rapid sketching exercise for quick idea generation.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-005', title: 'SCAMPER Technique', description: 'Apply SCAMPER technique to existing products for innovative improvements.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-006', title: 'Design Thinking Workshop', description: 'Lead design thinking workshop with empathy mapping and ideation phases.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-007', title: 'Feature Prioritization', description: 'Prioritize features using MoSCoW method and impact-effort matrix.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-008', title: 'Value Proposition Canvas', description: 'Create value proposition canvas to align product features with user needs.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-009', title: 'Business Model Canvas', description: 'Develop business model canvas for new product or service concepts.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-010', title: 'Competitive Differentiation', description: 'Identify competitive differentiation opportunities and unique value propositions.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-011', title: 'Innovation Workshop', description: 'Facilitate innovation workshop with future-thinking and trend analysis.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-012', title: 'Problem-Solution Fit', description: 'Validate problem-solution fit with user interviews and market research.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-013', title: 'MVP Definition', description: 'Define minimum viable product features and development roadmap.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-014', title: 'User Story Workshop', description: 'Create user stories and acceptance criteria for development planning.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' },
  { id: 'idea-015', title: 'Design Vision Workshop', description: 'Develop design vision and principles for product development alignment.', category: 'Ideation', tags: ['#vibe'], model: 'GPT' }
];

let ALL_PROMPTS = [];
let currentCategory = 'All';
let currentOffset = 0;
let FAVORITES = [];

// Configuration
const BATCH_SIZE = 5;
const CATEGORIES = ['All', 'My Library', 'UX Research', 'Accessibility', 'Prototyping', 'User Testing', 'Ideation'];

// Show the plugin UI
function showUI() {
  figma.showUI(__html__, {
    width: 300,
    height: 600
  });
}

// Filter prompts by category
function filterPromptsByCategory(prompts, category) {
  console.log(`Filtering ${prompts.length} prompts for category: ${category}`);
  if (category === 'All') {
    console.log('Returning all prompts:', prompts.length);
    return prompts;
  }
  const filtered = prompts.filter(prompt => prompt.category === category);
  console.log(`Filtered to ${filtered.length} prompts for category: ${category}`);
  return filtered;
}

// Get prompts with pagination and category filtering
function getPrompts(offset = 0, limit = BATCH_SIZE, category = 'All') {
  const filteredPrompts = filterPromptsByCategory(ALL_PROMPTS, category);
  const prompts = filteredPrompts.slice(offset, offset + limit);
  const totalCount = filteredPrompts.length;
  const hasMore = offset + limit < totalCount;
  
  return {
    prompts,
    totalCount,
    hasMore,
    offset,
    limit,
    category
  };
}

// Get all prompts for search functionality
function getAllPrompts() {
  return {
    prompts: ALL_PROMPTS,
    totalCount: ALL_PROMPTS.length,
    hasMore: false,
    offset: 0,
    limit: ALL_PROMPTS.length,
    category: 'All'
  };
}

// Handle messages from the UI
figma.ui.onmessage = async (msg) => {
  switch (msg.type) {
    case 'load-prompts':
      // Send first 5 prompts, filtered by category
      currentOffset = 0;
      currentCategory = msg.category || 'All';
      console.log(`Loading prompts for category: ${currentCategory}, total prompts: ${ALL_PROMPTS.length}`);
      
      const initialData = getPrompts(currentOffset, BATCH_SIZE, currentCategory);
      console.log(`Filtered prompts for ${currentCategory}:`, initialData.prompts.length, 'out of', initialData.totalCount);
      
      figma.ui.postMessage({
        type: 'prompts-loaded',
        prompts: initialData.prompts,
        totalCount: initialData.totalCount,
        hasMore: initialData.hasMore,
        offset: initialData.offset,
        limit: initialData.limit,
        category: initialData.category
      });
      
      if (ALL_PROMPTS.length > HARD_CODED_FALLBACK.length) {
        console.log('Prompts loaded from storage');
      } else {
        console.log('Using hardcoded prompts');
      }
      console.log(`Prompts sent: ${initialData.prompts.length} for category: ${currentCategory}`);
      currentOffset = BATCH_SIZE;
      break;

    case 'load-all-prompts':
      console.log('Loading all prompts for search functionality');
      const allData = getAllPrompts();
      
      figma.ui.postMessage({
        type: 'prompts-loaded',
        prompts: allData.prompts,
        totalCount: allData.totalCount,
        hasMore: allData.hasMore,
        offset: allData.offset,
        limit: allData.limit,
        category: allData.category
      });
      
      console.log(`All prompts sent: ${allData.prompts.length} for search`);
      break;

    case 'load-more':
      // Send next 5 prompts for the active category
      const moreData = getPrompts(currentOffset, BATCH_SIZE, currentCategory);
      
      if (moreData.prompts.length === 0) {
        figma.ui.postMessage({
          type: 'no-more-prompts',
          message: 'All prompts have been loaded'
        });
        console.log('No more prompts available');
      } else {
        figma.ui.postMessage({
          type: 'more-prompts-loaded',
          prompts: moreData.prompts,
          totalCount: moreData.totalCount,
          hasMore: moreData.hasMore,
          offset: moreData.offset,
          limit: moreData.limit,
          category: moreData.category
        });
        
        console.log(`Prompts sent: ${moreData.prompts.length} for category: ${currentCategory}`);
        currentOffset += BATCH_SIZE;
        
        if (!moreData.hasMore) {
          console.log('All prompts loaded for category:', currentCategory);
        }
      }
      break;

    case 'add-prompt':
      // Save new prompt to figma.clientStorage
      try {
        const newPrompt = {
          id: `custom-${Date.now()}`,
          title: msg.data.title,
          description: msg.data.description,
          category: msg.data.category,
          tags: ['#vibe'],
          model: 'GPT',
          createdAt: new Date().toISOString()
        };

        // Add to current prompts array
        ALL_PROMPTS.unshift(newPrompt); // Add to beginning
        
        // Save to storage
        await figma.clientStorage.setAsync('prompts', ALL_PROMPTS);
        
        console.log('Prompt added:', newPrompt.title);
        figma.notify(`Added "${newPrompt.title}" to prompts`);
        
        // Reload prompts for the active category
        currentOffset = 0;
        const updatedData = getPrompts(currentOffset, BATCH_SIZE, currentCategory);
        figma.ui.postMessage({
          type: 'prompts-loaded',
          prompts: updatedData.prompts,
          totalCount: updatedData.totalCount,
          hasMore: updatedData.hasMore,
          offset: updatedData.offset,
          limit: updatedData.limit,
          category: updatedData.category
        });
        
        currentOffset = BATCH_SIZE;
      } catch (error) {
        console.error('Failed to add prompt:', error);
        figma.notify('Failed to add prompt');
      }
      break;

    case 'send-to-make':
      // Notify placeholder
      console.log('Sending to Make API:', msg.data);
      figma.notify('Data sent to Make API successfully!');
      
      figma.ui.postMessage({
        type: 'make-api-success',
        message: 'Data sent to Make API (placeholder)'
      });
      break;

    case 'load-favorites':
      try {
        const storedFavorites = await figma.clientStorage.getAsync('favorites');
        FAVORITES = Array.isArray(storedFavorites) ? storedFavorites : [];
        console.log('Favorites loaded:', FAVORITES.length);
        
        figma.ui.postMessage({
          type: 'favorites-loaded',
          favorites: FAVORITES
        });
      } catch (error) {
        console.error('Failed to load favorites:', error);
        FAVORITES = [];
        figma.ui.postMessage({
          type: 'favorites-loaded',
          favorites: []
        });
      }
      break;

    case 'save-favorites':
      try {
        FAVORITES = msg.favorites || [];
        await figma.clientStorage.setAsync('favorites', FAVORITES);
        console.log('Favorites saved:', FAVORITES.length);
      } catch (error) {
        console.error('Failed to save favorites:', error);
      }
      break;

    case 'delete-prompt':
      try {
        const promptId = msg.data.id;
        const promptIndex = ALL_PROMPTS.findIndex(p => p.id === promptId);
        
        if (promptIndex !== -1) {
          const deletedPrompt = ALL_PROMPTS[promptIndex];
          ALL_PROMPTS.splice(promptIndex, 1);
          
          // Save updated prompts to storage
          await figma.clientStorage.setAsync('prompts', ALL_PROMPTS);
          
          console.log('Prompt deleted:', deletedPrompt.title);
          figma.notify(`Deleted "${deletedPrompt.title}"`);
          
          // Reload prompts for the current active category
          currentOffset = 0;
          const updatedData = getPrompts(currentOffset, BATCH_SIZE, currentCategory);
          figma.ui.postMessage({
            type: 'prompts-loaded',
            prompts: updatedData.prompts,
            totalCount: updatedData.totalCount,
            hasMore: updatedData.hasMore,
            offset: updatedData.offset,
            limit: updatedData.limit,
            category: updatedData.category
          });
          
          currentOffset = BATCH_SIZE;
        } else {
          console.warn('Prompt not found for deletion:', promptId);
          figma.notify('Prompt not found');
        }
      } catch (error) {
        console.error('Failed to delete prompt:', error);
        figma.notify('Failed to delete prompt');
      }
      break;

    case 'close-plugin':
      figma.closePlugin();
      break;

    default:
      console.warn('Unknown message type:', msg.type);
  }
};

// Initialize the plugin
async function init() {
  console.log('PromptCraft plugin initialized');

  try {
    const stored = await figma.clientStorage.getAsync('prompts');
    if (Array.isArray(stored) && stored.length > 0) {
      ALL_PROMPTS = stored;
      console.log(`Prompts loaded from storage: ${ALL_PROMPTS.length}`);
      console.log('Sample prompt:', ALL_PROMPTS[0]);
    } else {
      ALL_PROMPTS = HARD_CODED_FALLBACK;
      console.log('Using hardcoded prompts:', ALL_PROMPTS.length);
      console.log('Sample prompt:', ALL_PROMPTS[0]);
    }
  } catch (e) {
    ALL_PROMPTS = HARD_CODED_FALLBACK;
    console.log('Using hardcoded prompts due to error:', e);
    console.log('Sample prompt:', ALL_PROMPTS[0]);
  }

  // Load favorites
  try {
    const storedFavorites = await figma.clientStorage.getAsync('favorites');
    FAVORITES = Array.isArray(storedFavorites) ? storedFavorites : [];
    console.log('Favorites loaded:', FAVORITES.length);
  } catch (e) {
    FAVORITES = [];
    console.log('No favorites found, starting fresh');
  }

  // Show the UI
  showUI();

  // Inform UI of readiness
  figma.ui.postMessage({
    type: 'plugin-ready',
    totalPromptCount: ALL_PROMPTS.length,
    batchSize: BATCH_SIZE,
    categories: CATEGORIES
  });
}

// Start the plugin
init();