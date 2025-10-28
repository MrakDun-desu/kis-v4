# Feature Roadmap for KIS

1. Sale items, store items, transactions, cashboxes, users, categories
2. Prices, price histories, automatic price calculations, prestige tracking, price checks, stock
   takings
3. Containers, container items, pipes

-- Basic functionality --

4. Layouts
5. Open transactions
6. Modifiers, modification amounts
7. Discounts, discount usages

## Implementation details

Only one payment price should be stored per item, other currencies should be computed based on a
table. If a currency isn't in the table, it isn't supported for paying.

Store item price should be stored. But optionally on adding items, you should be able to recalculate
the price to reflect the average of all the purchased store items. In the store item UI, you should
be able to see the difference between the average bought price for the store item and the current
selling price to let you know that the price needs to be updated.

Sale item price should be defined as the sum of the prices of all the store items included in it
multiplied by their amounts, plus some margin

## Endpoint guidelines

- Get all gets _just_ the entity information from the entity itself
- Get single gets the internal entity information, child collections and single navigations
- Related collections that aren't children of the entity or are possible to get too big
  should be queried by separate queries with relevant filters (and paged)
