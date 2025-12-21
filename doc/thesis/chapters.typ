#import "custom-bibliography.typ": custom-cite
#import "common.typ": *

= Introduction

The goal of this thesis was to study and redesign the information system used by the Kachna
Student Club #footnote(link("https://su.fit.vut.cz/kachna/")).

#bigskip()

Any organization that handles products and their sales has a specific set of requirements for the
type of products it specializes in. A lot of this is shared for all such businesses -- product
storage tracking, price specification and tracking, sales
management and others. Systems to manage such businesses are pretty complex already and need
to be secure and consistent. The Student Club however also has some specific requirements that
are not easy to find in general sales systems.
For example, the club functions in multiple different ways:

- Most of the time, it's open in "bar" mode, selling beer from kegs and offering a big
  selection of snacks.
- Once per week, it's open in "teahouse" mode, mainly preparing and selling teas
  and maintaining a quiet, relaxed atmosphere. During this mode, the employees are
  also expected to bring the orders to individual tables.
- Sometimes, it's also open for special occasions, and it serves only small subset of the
  usual catalogue. During these events, the orders are usually all made on the name of a single
  person instead of a person ordering and then paying right away as usual.

On top of this, some of the products sold are more complex than others -- for example, different
beer kegs might be available on different days, and it's also necessary to track the beer amounts
of individual kegs.

Also, unlike most organizations, the Student Club is non-profit and requires voluntary contributions
from members to keep running. Because of this, it's absolutely necessary
to track the amounts of individual contributions. The contributions are also
used for gamification purposes to encourage members to support the growth of the club.

Because of all these requirements, the ideal solution needs to be custom-made. In the past,
there have been attempts to make such a system, but the members of the Student Union have not
been satisfied with them. Some of the modules of the system are satisfactory, however,
and for a new solution, it's better to integrate with them than to implement from scratch.

Such systems are:

- KIS (Kachna Information System) Food, which handles tracking long-duration orders such
  as making toasts, and displaying information about them on monitors.
- KIS Auth, which handles authentication and authorization. It also integrates with the EduID
  academic identity federation for ease of signing up.

In the following chapters, I will:

- State the requirements for the system as specified by the Student Union in chapter @reqs
- Analyze the requirements and formalize them as UML diagrams in chapter @analysis
- Design the individual parts of the system in chapter @design
- And finally sum up the work in chapter @concl

= Requirements and goals <reqs>

This chapter summarizes the necessary and optional requirements that the individual parts
of the system must fulfill. It will be referenced in the further chapters and influence the
way the system was implemented.

== Informal specification

The requirements are very similar to any sales management application, with some additional
requirements on top. There is no technology requirement, though some technologies are easier
to integrate with the existing solutions than others.

Currently, the Student Club uses a solution that works, but is quite insufficient in several
areas that this project is supposed to improve upon. Some parts of the old system, such as
authentication and order management are separate from the main old application and will be kept,
but the main system used to manage products and sales will be completely replaced.
It would be ideal if the data was able to easily be transferred to a new database,
but since the amount of products is not that large, it is possible to rewrite data manually
for better user experience later.

The full informal specification includes various levels of necessity:

- Absolute necessities:
  - Tracking the amount and time of changes of product amounts in the stores
  - Tracking price of the individual products over time
  - Tracking individual sale transactions, how much was paid for each and how much did the
    customer voluntarily contribute to the club at which cash-box
  - Tracking the currently open kegs and the amounts of product in them, as well as
    which pipe they are opened for
- Important features and improvements:
  - Being able to differentiate individual kegs from each other and track how much was actually
    used on average from which type of keg
  - Integrating with the existing authentication system that lets students register as members
    of the Student Club with EduID identification
  - Integrating with the existing order-tracking system that also allows printing order numbers
    and table numbers for individual teahouse orders
  - Auditing database changes
  - Letting customers have an "open sale transaction" where they don't pay for the product
    right away. This allows the barman to update the stored amounts of products without
    finalizing the sale transaction completely
  - Separating products into "store items" which are bought and stored, and "sale items" which
    are sold and can be composed of multiple store items. For example, one toast is usually
    composed of bread, ham, cheese and ketchup
  - Adding "modifiers" which can be associated with different sale items to more easily alter
    their composition and price instead of storing every variation of a sale item in the database
  - Point-of-Sales user interface with static positioning of sale items, where existing layouts of
    items don't change just by adding more items. This can be achieved by storing layouts instead of
    sorting the items
- Nice-to-haves:
  - Managing dynamic discounts and tracking their usage by different customers. This also includes
    deprecating discounts that no longer apply
  - Being able to process payments in different currencies

== Requirements for a modern web application

Any modern web application is expected by users to have certain characteristics. This is the same
for this project, and I'm mentioning them explicitly to not forget their importance.

General requirements that this application should support include:

- Fluent UI - no delays for basic actions, fast loading speeds
- Good user experience - intuitive UI design, easily recognizable patterns
- Good security
- Flexible design - it should be easy to add new functions, endpoints and change business logic
- Well-maintainable design - the libraries used should be well-supported and updated frequently to
  prevent security problems and deprecations
- Responsivity - ability to function across a range of typical screen resolutions
- Multi-platform functionality - ability to function across various modern browsers

The requirements for responsivity and multi-platform functionality can be skipped for the
Point-of-Service part of the application, since that part will always be used on specific hardware
with a specific resolution.

= Requirement analysis <analysis>

This chapter is dedicated to combining all the requirements and analyzing them. The results are
resulting technology restrictions, use-case diagrams for various users of the system and
entity relation diagrams that show connections between individual parts of the system.

== Specification analysis

This section describes the requirements more clearly and states how they will be met in
the finished application.

=== Basic entities

According to the informal specification (from section @reqs), multiple different entities need to be managed:

- *Store items*, which directly correspond to products stored in the individual stores used by the
  student club
- *Sale items*, which are composed of various amounts of store items and correspond to units that are
  actually sold to customers
- *Store item cost records*, which represent costs of the store items in particular point in time.
  These costs will also be used to compute the costs of sale items
- *Stores*, which correspond to places where the store items are typically stored, for example at the
  club storage space, or directly at the bar, or in different rooms according to where the student
  club is currently open
- *Store transactions*, which track how many store items have been added to a particular store or
  removed from it. In case of product addition, store transactions also track how much the added
  store items cost
- *Containers*, which correspond to beer kegs -- they track the amount of a specific store item
  (usually a certain kind of beer)
- *Pipes*, which correspond to places where kegs can currently be at use
- *Users*, which will not actually be managed by this system, but will be used to track
  responsibilities of student union members, contribution amounts of student club members and their
  discount usages
- *Modifiers*, which can be associated with different sale items to alter their composition and
  cost,
- *Sale transactions*, which track how many sale items with which modifiers have been sold by which
  union member to which student club member. They can also be linked to store transactions which
  they initiate (most sale transactions should initiate one store transaction to remove a certain
  amount of items from a specific store)
- *Cash-boxes*, which track amount of sale money and contribution money at a specific real-life
  cash-box
- *Discounts* that track custom logic associated with applying discounts to sale transactions

Most of these entities need to support basic CRUD #footnote[CRUD - basic functionality of persistent
  storage - Create, Read, Update, Delete] functionality. Notable exceptions to this are transactions,
which can't be created and updated simply, but require more complex business logic which will be
specified later.

Some of the other items do not need delete functionality, as they should always stay in the database
to let the users inspect their changes over time - for example store item costs,
transactions, containers and discounts.

= Design <design>

== Database

== Sales API

== User interface


= Conclusion <concl>
