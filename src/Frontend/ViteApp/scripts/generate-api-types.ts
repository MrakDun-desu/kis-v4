import { readFileSync, writeFileSync } from "node:fs";

// path must be relative to the root project folder
const spec = JSON.parse(readFileSync("../../../kisv4-openapi-doc.json", 'utf-8'));

const schemaNames = Object.keys(spec.components.schemas);

const lines = [
  `import type { components } from './apiSchema.ts';`,
  `type Schemas = components['schemas'];`,
  ``,
  ...schemaNames.map(name => `export type ${name} = Schemas['${name}'];`)
];

writeFileSync("./src/api/apiTypes.ts", lines.join("\n"));

