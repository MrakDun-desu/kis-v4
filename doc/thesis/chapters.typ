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

- Talk about modern information systems and what requirements are usually expected when implementing
  them, as well as discuss what security measures are usually employed, in chapter @theory
- Familiarize the reader with the current state of the information systems used by the Kachna
  Student Club in chapter @current
- Analyze the requirements of the Student Union and formalize them as UML diagrams in chapter
  @analysis
- Design the individual parts of the system in chapter @design
- And finally sum up the work in chapter @concl


= Modern information systems <theory>

== Requirements for a modern web appliation

== Commonly used security practices

= Current state of the information system <current>

== Sales subsystem

== Operator subsystem

== Admin subsystem

== Food management subsystem

== Authentication subsystem

= Use-case analysis <analysis>

== Informal specification

== Use-case diagrams

= Application design <design>

== Technology choices

== Entity design

== Application architecture

== User interface design

= Conclusion <concl>
