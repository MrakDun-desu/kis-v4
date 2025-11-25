#import "vut_thesis.typ": vut_thesis
#import "common.typ": todo

// Change chapters.typ to edit chapters, change appendices.typ to edit appendices.
// Everything else should already be set up.
//
// ČSN ISO 690 norm doesn't work properly with what Typst offers, so I made a completely custom
// bibliography and cite functions in custom-bibliography.typ. They aren't too mature, but they work
// well enough for the usecase.

#let cs(content) = text(lang: "cs", content)

#vut_thesis(
  lang: "en", // primary language of the thesis
  lang2: "cs", // secondary language
  project_type: [Master's Thesis],
  project_type2: cs[Diplomová práce],
  date: datetime.today(),
  title: [Modular Student Club Management System with Integration Into Existing Infrastructure],
  title2: cs[Modulární systém pro správu studentského klubu s integrací do stávající infrastruktury],
  department: [Department of Information Systems],
  department2: cs[Ústav informačních systémů],
  author_p: [Bc.],
  author_name: [Marek],
  author_surname: [Dančo],
  author_a: [],
  supervisor_p: [Ing.],
  supervisor_name: [Ondřej],
  supervisor_surname: [Ondryáš],
  supervisor_a: [],
  keywords: [
    information system, product management, sales, C\#, React, ASP.NET Core, Entity Framework
    Core, modular design, web application, full-stack
  ],
  keywords2: cs[
    informační systém, správa produktů, prodej, C\#, React, ASP.NET Core, Entity Framework
    Core, modulární návrh, webová aplikace, full-stack
  ],
  abstract: [
    This thesis deals with the design and implementation of a modular information system
    for a student club. It handles store management, product and
    composite product management, automated price calculation, tracking
    voluntary contributions, and offers two user interfaces, one for administration and one for
    sales. Backend has been implemented with ASP.NET Core and it integrates with an existing custom
    authentication server using Duende IdentityServer. Frontend has been implemented with React and
    built with Vite.
  ],
  abstract2: cs[
    Tato práce se zabývá návrhem a implementací modulárního informačního systému pro studentský
    klub. Obsahuje možnosti správy skladů, správy produktů a zložených produktů, automatické
    počítání ceny, sledování dobrovolných příspěvků a dvě uživatelská rozhraní, jedno pro
    administraci a druhé pro samotný prodej. Backend byl implementován v ASP.NET Core a integruje s
    existujícím autentizačním serverem, který používá Duende IdentityServer. Frontend byl
    implementován v Reactu a sestaven programem Vite.
  ],
  declaration: [
    I declare that I have worked on this thesis independently under the supervision of
    Ing. Ondřej Ondryáš. I have stated all literary sources, publications and other resources that I
    have used.
  ],
  acknowledgments: [
    #todo[Todo]
  ],
)
