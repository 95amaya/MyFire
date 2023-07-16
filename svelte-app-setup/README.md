# Svelte + TS + Vite

This template should help get you started developing with Svelte and TypeScript in Vite.

## Steps used to create app
### Initialize App
1. Run `npm init vite@latest` and choose svelte and TS options
2. `CD` into newly create project
3. `npm install && npm run dev` to install packages and run app
4. Verify this works in browser 

### Add Dependency Injection Support
1. Run `npm install inversify reflect-metadata`
2. Setup inversify IoC test
3. Run `npm run dev` to verify inversify is integrated successfully
4. Add `"experimentalDecorators": true` to `tsconfig.json` to fix typescript linting issues 
5. Run `npm run dev` to verify typescript is updated successfully

### Add Tailwind Support
Follow `https://tailwindcss.com/docs/guides/sveltekit`
1. Install Tailwind by running `npm install -D tailwindcss postcss autoprefixer && npx tailwindcss init -p`
2. Enable use of PostCSS in `<style>` blocks
3. Configure Template Paths
4. Add Tailwind Directive to `app.css`
   1. Configure VS Code to use `tailwindcss` language model to remove warnings
5. Replace all existing styles with Tailwind Hello World styles
6. Run `npm run dev` to verify tailwind is integrated successfully

### Add DaisyUI Support
1. Follow `https://daisyui.com/docs/install/`
2. Add Themes with `https://daisyui.com/docs/themes/`
3. Update App to use daisyui css selectors
4. Run `npm run dev` to verify daisyui is integrated successfully

## Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Svelte](https://marketplace.visualstudio.com/items?itemName=svelte.svelte-vscode).


## Technical considerations

**Why use this over SvelteKit?**

- It brings its own routing solution which might not be preferable for some users.
- It is first and foremost a framework that just happens to use Vite under the hood, not a Vite app.

This template contains as little as possible to get started with Vite + TypeScript + Svelte, while taking into account the developer experience with regards to HMR and intellisense. It demonstrates capabilities on par with the other `create-vite` templates and is a good starting point for beginners dipping their toes into a Vite + Svelte project.

Should you later need the extended capabilities and extensibility provided by SvelteKit, the template has been structured similarly to SvelteKit so that it is easy to migrate.

**Why `global.d.ts` instead of `compilerOptions.types` inside `jsconfig.json` or `tsconfig.json`?**

Setting `compilerOptions.types` shuts out all other types not explicitly listed in the configuration. Using triple-slash references keeps the default TypeScript setting of accepting type information from the entire workspace, while also adding `svelte` and `vite/client` type information.

**Why include `.vscode/extensions.json`?**

Other templates indirectly recommend extensions via the README, but this file allows VS Code to prompt the user to install the recommended extension upon opening the project.

**Why enable `allowJs` in the TS template?**

While `allowJs: false` would indeed prevent the use of `.js` files in the project, it does not prevent the use of JavaScript syntax in `.svelte` files. In addition, it would force `checkJs: false`, bringing the worst of both worlds: not being able to guarantee the entire codebase is TypeScript, and also having worse typechecking for the existing JavaScript. In addition, there are valid use cases in which a mixed codebase may be relevant.

**Why is HMR not preserving my local component state?**

HMR state preservation comes with a number of gotchas! It has been disabled by default in both `svelte-hmr` and `@sveltejs/vite-plugin-svelte` due to its often surprising behavior. You can read the details [here](https://github.com/rixo/svelte-hmr#svelte-hmr).

If you have state that's important to retain within a component, consider creating an external store which would not be replaced by HMR.

```ts
// store.ts
// An extremely simple external store
import { writable } from 'svelte/store'
export default writable(0)
```