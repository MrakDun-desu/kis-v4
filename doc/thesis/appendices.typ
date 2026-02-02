#import "custom-bibliography.typ": custom-cite
#import "common.typ": *

= Entity relationship diagram <er_diagram>

The Figures @er_diagram_products, @er_diagram_transactions, @er_diagram_layouts and
@er_diagram_containers depict the data model of the new KIS Sales back-end, as designed according to
the user requirements and some assumptions made during analysis. The diagram has been split into
multiple figures because of its complexity.

#figure(
  image("figures/er_diagram_products.pdf"),
  caption: flex-caption(
    long: [Entity relationship diagram for the new KIS Sales -- Products],
    short: [Entity relationship diagram -- Products],
  ),
) <er_diagram_products>

#figure(
  image("figures/er_diagram_transactions.pdf", height: 62%),
  caption: flex-caption(
    long: [Entity relationship diagram for the new KIS Sales -- Transactions],
    short: [Entity relationship diagram -- Transactions],
  ),
) <er_diagram_transactions>

#figure(
  image("figures/er_diagram_layouts.pdf"),
  caption: flex-caption(
    long: [Entity relationship diagram for the new KIS Sales -- Layouts],
    short: [Entity relationship diagram -- Layouts],
  ),
) <er_diagram_layouts>

#figure(
  image("figures/er_diagram_containers.pdf"),
  caption: flex-caption(
    long: [Entity relationship diagram for the new KIS Sales -- Containers (Kegs
      for tapped drinks)],
    short: [Entity relationship diagram -- Containers],
  ),
) <er_diagram_containers>

