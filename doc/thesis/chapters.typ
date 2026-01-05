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
  #footnote(link("https://www.eduid.cz")[EduId.cz])
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

This chapter summarizes all the typical requirements that a modern information system is expected to
fulfill, that are relevant to this project. Special attention is given to security practices
(@security), as this information system needs to integrate with more complex security solutions,
such as the EduId #footnote(link("https://www.eduid.cz", [EduId.cz])) academic identity federation.

== Requirements for a modern web application

These are individual characteristics of a software product that can be used to judge quality of
software, taken from the ISO/IEC 25010 standard #custom-cite("iso25010"). For each of the
characteristics, I will either specify what is required of this project, or why the characteristic
isn't very relevant to this particular software.

=== Functional suitability

The capability of the product to meet the functional needs of its users.

In the chapter
@analysis, I will specify what exactly these needs are for this project. Some of the user
requirements are mandatory, while some are only nice-to-have and might be skipped either due to lack
of time or to focus on more important user needs.

=== Performance efficiency

The capability of a product to perform the functions within specified time and be efficient in the
use of its resources.

A modern web application is expected to perform most of its task in time perceptible as instant by
human eyes, and to provide visual feedback for the tasks that are required to take significant
amount of time.

It is also expected of the system to be able to respond to all the requests the users are expected
to have -- for this project, however, the scalability isn't much of an issue, since the School Club
is a fairly small organization with the amount of active members in dozens. The product is not
expected to be deployed in a large-scale scenario, so the capacity requirement is fairly small.

=== Compatibility

The capability of a product to exchange information with other products and to perform its
functions while sharing environment and resources with them.

This product is expected to work with the other modules of the Kachna Information System and to be
further extended by other systems in the future.

=== Interaction capability

The capability of a product to be interacted with by users. It also includes the following:
- *appropriateness recognizability* - whether the product can be recognized by the users as
  appropriate for their needs,
- *learnability* - whether the product's functionality can be easily learned,
- *self-descriptiveness* - whether the product is able to make its capabilities obvious to the
  users,
- and *user error protection* - whether the product is able to prevent operation errors from the
  users.

It is absolutely necessary that the product can be recognized as appropriate. The users should be
able to work with it and improve their workflow as fast as possible.

It is also very necessary to prevent operation errors, or in case of an error, helping the user
resolve it as fast as possible via changing or removing the effects of the error such that the
resulting state of the system correctly corresponds to the state of the real world.

Learnability and self-descriptiveness aren't as crucial to this product, as it is expected for the
new operators to be schooled by the more experienced users, but it would still be very useful if the
need for schooling was as small as possible, and the users were able to start using the system
without too much help.

#bigskip()

There are other parts of interaction capability that should be mentioned but aren't as relevant to this
product for various reasons:
- *user engagement* - it is not necessary for the users to feel particularly engaged by the product as
  it is required for their work. Engagement is mostly important when users interact with the product
  in their free time for leisure,
- *inclusivity* - inclusivity is not very important for this particular product because it is known
  that only the students of FIT VUT will be using it, and as such it's not necessary to think about
  too wide of a spectrum of users,
- *user assistance* - it is not very important to assist users with particular needs or disabilities
  to the clients, because the chances of such users needing to use the product are exceedingly low.
  It is more important to work on other functionality of the product before considering this.

=== Reliability

The capability of a product to perform its functions without interruptions or failures.

This product should function without any failures, and it should be available under normal use. It
should also be possible to recover the state of the system from an earlier point in time.

When hardware faults occur, it is acceptable for the product to stop functioning for a short period
of time, but it should be able to start functioning again as fast as possible.

=== Security

The capability of a product to protect information and data, and to defend against attack patterns
by malicious actors. This includes the following:
- *confidentiality* - capability to ensure that only authorized users access protected data,
- *integrity* - capability to ensure that the data cannot be modified or deleted by unauthorized
  users or computer error,
- *non-repudiation* - capability to prove that actions have taken place,
- *accountability* - traceability of actions within the system to a specific entity,
- *authenticity* - capability to prove that subject or resource is the one it claims to be,
- and *resistance* - capability to sustain operations even while under attack.

Confidentiality, integrity, accountability and authenticity are very important to this system,
especially since it includes information about exchange of money between students and the Student
Club.

Non-repudiation and resistance are still important, but not as vital, since it is not expected that
anyone would want to attack the system. And while the system holds information about exchange of
money, it is never expected to be treated as the source of truth, only as an accounting helper.

=== Maintainability

The capability of a product to be modified with effectiveness and efficiency. This includes:
- *modularity* - changes to one component shouldn't affect other components,
- *reusability* - capability of a product to be used in more than just one system,
- *modifiability* - capability to be modified without degrading product quality,
- and *testability* - capability to be objectively and feasibly tested for requirements.

The product should be modular, and it should be easy to change parts of it without affecting other
components. It should also be easily modifiable without introducing bugs.

Reusability is not very important, as it is not planned to use the product in more scenarios than
the expected scenarios.

The product should also be easily tested, and it should be easily verifiable if it serves its
function correctly.

=== Flexibility

The capability of a product to be adapted to changes in requirements, contexts of use, or system
environment.

Most parts of the system do not require to be deployed in different environments - the back-end and
database are expected to stay the same, and the point-of-service front-end is also only expected to
be used from a specific hardware. Context of use should also stay mostly the same.

The only part of the system that really needs to be flexible is the administrative web application,
which should be able to run across various modern browsers, but it is again sufficient to be able to
run it on desktops. Enabling mobile users to use the administrative application would be
nice-to-have, but it is not a hard requirement.

== Commonly used security practices <security>

Commonly used modern security practices have shifted from simple username and password authentication
to delegated authorization using OAuth 2.0 for authorization and OpenID Connect for authentication.
Both of these protocols are built on
top of the HTTPS protocol as a communication channel and transfer authorization data in the JSON Web
Token format.

This project uses delegated authentication with OpenID Connect and it relies on the previously
created subsystem "KIS Auth".

=== OAuth 2.0

OAuth 2.0 is an authorization framework that enables a third-party application to obtain limited
access to an HTTP service, either on behalf of a resource owner by orchestrating an approval
interaction between the resource owner and the HTTP service, or by allowing the third-party
application to obtain access on its behalf #custom-cite("oauth").

OAuth 2.0 is the most commonly used authorization protocol in modern web applications. It defines a
way for client application to get access to resources via *access token* - a string denoting a
specific scope, lifetime and other attributes.

Access tokens should always be short-lived or single use, so for simplifying obtaining additional
ones after the first sign-on, OAuth 2.0 also allows the use of a *refresh token*, which can live for
longer and can be used to renew an access token.

It defines multiple flows for authorization:
- *authorization code grant*, which is used to obtain both access tokens and refresh tokens and is
  optimized for confidential clients,
- *implicit grant*, which is used to obtain access tokens only, and is optimized for public clients
  known to operate a particular redirection URI,
- *resource owner password credentials grant*, which is suitable in cases where the resource owner
  has a trust relationship with the client, and is used to obtain the resource owner's credentials,
- and *client credentials grant*, which is used to request an access token only using client
  credentials.

In modern systems, some of these flows have already been deprecated, like the implicit grant.
Instead, for authenticating users themselves, only the authorization code grant is used, usually
with PKCE #footnote[Proof-Key for Code Exchange] to prevent attackers from successful authorization
even if they were to intercept the authorization code itself.

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
