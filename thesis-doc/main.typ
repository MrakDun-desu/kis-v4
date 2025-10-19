#import "vut_thesis.typ": vut_thesis

// primary language of the thesis
#let lang = "en"
// secondary language is a function that also gets used in the template itself
// if primary language isn't english, secondary language should always be english
// if primary language isn't czech, secondary language should always be czech
#let lang2(body) = text(lang: "cs", body)

// Change chapters.typ to edit chapters, change appendices.typ to edit appendices.
// Everything else is already set up. Some things are only set up for English, so there will be some
// fiddling to get Czech/Slovak to work.
//
// ČSN ISO 690 norm doesn't work properly with what Typst offers, so I made a completely custom
// bibliography and cite functions in custom-bibliography.typ. They aren't too mature, but they work
// well enough for the usecase

#vut_thesis(
  lang,
  lang2,
  project_type: [Master's Thesis],
  project_type2: lang2[Diplomová práce],
  date: datetime.today(),
  title: [Modular Student Club Management System with Integration Into Existing Infrastructure],
  title2: lang2[Modulární systém pro správu studentského klubu s integrací do stávající infrastruktury],
  department: [Department of Information Systems],
  department2: lang2[Ústav informačních systémů],
  author_p: [Bc.],
  author_name: [Marek],
  author_surname: [Dančo],
  author_a: [],
  supervisor_p: [Ing.],
  supervisor_name: [Ondřej],
  supervisor_surname: [Ondryáš],
  supervisor_a: [],
  keywords: [
    Sem budou zapsána jednotlivá klíčová slova v českém (slovenském) jazyce
    oddělená čárkami.
  ],
  keywords2: [
    Sem budou zapsána jednotlivá klíčová slova v anglickém jazyce
    oddělená čárkami.
  ],
  abstract: [
    Do tohoto odstavce bude zapsán výtah (abstrakt) práce v českém (slovenském) jazyce.
  ],
  abstract2: [
    Do tohoto odstavce bude zapsán výtah (abstrakt) práce v českém (slovenském) jazyce.
  ],
  declaration: [
    Prohlašuji že jsem tuto bakalářskou práci vypracoval samostatně pod vedením pana X...
    Další informace mi poskytli...
    Uvedl jsem všechny literární prameny publikace a další zdroje, ze kterých jsem čerpal.
  ],
  acknowledgments: [
    V této sekci je možno uvést poděkování vedoucímu práce a těm, kteří poskytli odbornou pomoc
    (externí zadavatel, konzultant apod.).
  ],
)
