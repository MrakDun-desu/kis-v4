"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const node_fs_1 = require("node:fs");
// path must be relative to the root project folder
const spec = JSON.parse((0, node_fs_1.readFileSync)("../../../kisv4-openapi-doc.json", 'utf-8'));
const schemaNames = Object.keys(spec.components.schemas);
const lines = [
    `import type { components } from './apiSchema.ts';`,
    `type Schemas = components['schemas'];`,
    ``,
    ...schemaNames.map(name => `export type ${name} = Schemas['${name}'];`)
];
(0, node_fs_1.writeFileSync)("./src/api/apiTypes.ts", lines.join("\n"));
