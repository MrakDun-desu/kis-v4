#import "vut_thesis.typ": vut_thesis
#import "common.typ": cs

// Change chapters.typ to edit chapters, change appendices.typ to edit appendices.
// Everything else should already be set up.
//
// ČSN ISO 690 norm doesn't work properly with what Typst offers, so I made a completely custom
// bibliography and cite functions in custom-bibliography.typ. They aren't too mature, but they work
// well enough for the usecase.

#vut_thesis(
  "en", // primary language of the thesis
  // secondary language is a function that also gets used in the template itself
  // if primary language isn't english, secondary language should always be english
  // if primary language isn't czech, secondary language should always be czech
  "cs",
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
)
