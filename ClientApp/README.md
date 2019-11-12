# Front End

The front end (FE) was created with the .NET web api + react FE proejct template. The project was then altered to add TypeScript and Yarn as well as removing eslint/babel dependencies.

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
1. When calling the api ensure you use `/src/services/FetchWrapper.ts`. It will add the `trackingId` header to each request which the API uses for logging.
1. If you find a MUI component you want to use, it is recommended to create your own component with whatever customizations you want (see `/src/components/Button.tsx` for an example).
1. The MUI Table component is great, but requires either a lot of customization or a separate dependency for sorting, filtering, selection, etc.

## Dependencies

This is not all dependencies for the FE. This is a list of dependencies that were installed on top of the dependencies required by either CRA or the .NET template. This list also does not include devDependencies. All devDependencies are either Definitely Typed `@types/...` type definition modules or were installed by CRA.

| Depedency             | Description                                                                                 |
| --------------------- | ------------------------------------------------------------------------------------------- |
| `@material-ui`        | Component library implementing MUI UX ideals in React components.                           |
| `react-bootstrap`     | Used for grid layout. Provides `Row` and `Col` components for easy flexbox bootstrap stuff. |
| `lodash`              | Handy array and object functions.                                                           |
| `react-beautiful-dnd` | Drag and drop library written and used by Atlassian.                                        |
| `react-toastify`      | Toast message library.                                                                      |
| `typeface-roboto`     | Module containing Roboto typeface used in MUI components.                                   |
| `uuid`                | GUID generation for trackingId.                                                             |
