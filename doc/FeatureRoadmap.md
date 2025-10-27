# Feature Roadmap for KIS

1. Sale items, store items, transactions, cashboxes, layouts, users, categories
2. Prices, price histories, automatic price calculations, prestige tracking, price checks, stock
   takings
3. Containers, container items, container templates, pipes

-- Basic functionality --

4. Open transactions
5. Modifiers, modification amounts
6. Discounts, discount usages

## Implementation details

Only one payment price should be stored per item, other currencies should be computed based on a
table. If a currency isn't in the table, it isn't supported for paying.

Store item price should be stored. But optionally on adding items, you should be able to recalculate
the price to reflect the average of all the purchased store items. In the store item UI, you should
be able to see the difference between the average bought price for the store item and the current
selling price to let you know that the price needs to be updated.

Sale item price should be defined as the sum of the prices of all the store items included in it
multiplied by their amounts, plus some margin
