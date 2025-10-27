# KIS Usecase Meeting

- layouts as trees, not just as one-level grids
- sale items might not even need to have store items associated with them
- price for sale items might be better done as some kind of equation, not a fixed price
   - should be some kind of sum of the store item prices + sale margin
- barman needs to be able to transfer items from one store to another
   - barman can change the store status as they want to
- store items need to be somehow associated with physical items with different prices
   - or at least when adding items to the store, also add the price the items were bought for so the
     store item price can be changed
   - prices could be changed automatically when store items are bought, but only by some given
     amount
   - prices should also be able to be changed manually, and when the price changes a lot, a
     notification should be sent
- with open sale transaction, it needs to be necessary to open them even without a corresponding
  user, and those should be only allowed for some people. Also the normal open sale transactions,
  they should have some monetary limit on the amount of money they should store
- the additional money should be handled very carefully and separately from normal sale money
- additional money could be modelled as a separate cashbox
- with additional money, could be automatically counted how much they have to return to users
- for transactions without sale transaction items, it should be required to add a manual note
- with containers, it should be possible to just remove them from the pipe without throwing out the
  rest of the store items inside of the container
- with containers, it should be possible to choose if it's empty, bad or just removed for some time
   - if the container was bad, there should be a notification to a administrator
- with sale transactions, it's necessary to check if there should be something printed for the given
  sale item

## Possible extensions

- sale items based on other sale items
