# Softbot Cloud Platform Frontend

## Project Structure and Important Files

| Path                      | Description                                                                                        |
| ------------------------- | -------------------------------------------------------------------------------------------------- |
| `/public`                 | Public resources                                                                                   |
| `/src`                    | Source code                                                                                        |
| `/src/components`         | Top level or reusable components                                                                   |
| `/src/hooks`              | Custom hooks                                                                                       |
| `/src/icons`              | Icon components                                                                                    |
| `/src/services`           | Modules responsible for interacting with the API                                                   |
| `/src/views`              | Subfolders for each screen contain the component for the screen and any screen-specific components |
| `/src/index.tsx`          | React entrypoint and base component for the entire app. Routing is located here.                   |
| `/src/react-app-env.d.ts` | Reference to react-scripts types                                                                   |

## Gotchas and Other Such Things

1. Create React App requires isolated modules. If you change it, CRA _will change it back_ when you start the dev server.
1. The `tsconfig.json` option `compilerOptions.baseUrl` is set to enable easier importing. For example, instead of something like `import {Button} from '../../components'` you can cut it down to `import {Button} from 'components'`.
