#import "./custom-bibliography.typ": custom-bibliography
#import "./common.typ": cz

#let vut_thesis(
  lang,
  lang2,
  project_type: [Diplomová práce],
  project_type2: [Master's Thesis],
  date: datetime.today(),
  title: [Název práce],
  title2: [Thesis title],
  department: [Ústav informačních systémů],
  department2: [Department of Information Systems],
  author_p: [],
  author_name: [Name],
  author_surname: [Surname],
  author_a: [],
  supervisor_p: [Ing.],
  supervisor_name: [Name],
  supervisor_surname: [Surname],
  supervisor_a: [Phd.],
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
) = {
  if type(lang2) == str {
    lang2 = content => text(lang: lang2, content)
  }

  assert(
    type(lang2) == function,
    message: "Second language needs to be string or function",
  )

  // global style rules
  set page(
    paper: "a4",
    margin: (
      left: 3.46cm,
      right: 2.34cm,
      top: 2.5cm,
      bottom: 3.73cm,
    ),
  )
  set text(
    font: "New Computer Modern",
    lang: lang,
  )

  set par(justify: true, first-line-indent: 1.5em, spacing: 0.6em)

  show raw: set text(font: "LMMono10")
  show link: set text(rgb("#092eab"), font: "LMMono10")

  set heading(numbering: none, supplement: none)
  show heading: set par(first-line-indent: 0pt)
  show heading.where(depth: 1): set text(size: 25pt)
  show heading.where(depth: 1): self => {
    pagebreak()
    counter(math.equation).update(0)
    counter(figure.where(kind: image)).update(0)
    counter(figure.where(kind: table)).update(0)
    v(90pt)
    if self.numbering != none and self.supplement != none {
      text(
        size: 18pt,
        [#self.supplement #numbering(
            self.numbering,
            ..counter(heading).get(),
          )],
      )
    }
    block(above: 35pt, below: 48pt, self.body)
  }
  // used to replace h1 heading where pagebreak before them can't be used (outlines)
  let var_heading(content) = {
    block(above: 100pt, below: 35pt, content)
  }

  show heading.where(depth: 2): set text(size: 18pt)
  show heading.where(depth: 2): self => {
    v(17pt)
    if self.numbering != none {
      [#numbering(self.numbering, ..counter(heading).get()) ]
    }
    self.body
    v(12pt)
  }

  show heading.where(depth: 3): set text(size: 15pt)
  show heading.where(depth: 3): set block(below: 8pt)

  show ref: it => text(red, it)
  set ref(supplement: none)

  show figure: set block(above: 2em, below: 2em)

  set list(indent: 1em, spacing: 1.3em)
  show list: set block(above: 1.2em, below: 1.2em)
  set enum(indent: 1em, spacing: 1.3em)
  show enum: set block(above: 1.2em, below: 1.2em)

  set outline(depth: 2)
  show outline.entry: set outline.entry(fill: {
    h(1em)
    box(width: 1fr, repeat([.], gap: 0.5em))
    h(1em)
  })

  // fixed words in different languages
  context {
    let _university = () => if text.lang == "cs" {
      [Vysoké učení technické v Brně]
    } else if text.lang == "sk" {
      cz[Vysoké učení technické v Brně]
    } else if text.lang == "en" {
      [Brno university of technology]
    }
    let _faculty = () => if text.lang == "cs" {
      [Fakulta informačních technologií]
    } else if text.lang == "sk" {
      cz[Fakulta informačních technologií]
    } else if text.lang == "en" {
      [Faculty of Information Technology]
    }
    let _author = () => if text.lang == "cs" {
      [Autor]
    } else if text.lang == "sk" {
      [Autor]
    } else if text.lang == "en" {
      [Author]
    }
    let _supervisor = () => if text.lang == "cs" {
      [Vedoucí práce]
    } else if text.lang == "sk" {
      [Vedúci práce]
    } else if text.lang == "en" {
      [Supervisor]
    }
    let _abstract = () => if text.lang == "cs" {
      [Abstrakt]
    } else if text.lang == "sk" {
      [Abstrakt]
    } else if text.lang == "en" {
      [Abstract]
    }
    let _keywords = () => if text.lang == "cs" {
      [Klíčová slova]
    } else if text.lang == "sk" {
      [Kľúčové slová]
    } else if text.lang == "en" {
      [Keywords]
    }
    let _declaration = () => if text.lang == "cs" {
      [Prohlášení]
    } else if text.lang == "sk" {
      [Prehlásenie]
    } else if text.lang == "en" {
      [Declaration]
    }
    let _acknowledgments = () => if text.lang == "cs" {
      [Poděkování]
    } else if text.lang == "sk" {
      [Poďakovanie]
    } else if text.lang == "en" {
      [Acknowlegments]
    }
    let _contents = () => if text.lang == "cs" {
      [Obsah]
    } else if text.lang == "sk" {
      [Obsah]
    } else if text.lang == "en" {
      [Contents]
    }
    let _list_of_figures = () => if text.lang == "cs" {
      [Seznam obrázků]
    } else if text.lang == "sk" {
      [Zoznam obrázkov]
    } else if text.lang == "en" {
      [List of Figures]
    }
    let _chapter = () => if text.lang == "cs" {
      [Kapitola]
    } else if text.lang == "sk" {
      [Kapitola]
    } else if text.lang == "en" {
      [Chapter]
    }
    let _appendix = () => if text.lang == "cs" {
      [Příloha]
    } else if text.lang == "sk" {
      [Príloha]
    } else if text.lang == "en" {
      [Appendix]
    }

    let figure_supplement = auto
    let table_supplement = auto
    if text.lang == "sk" {
      figure_supplement = "Obrázok"
      table_supplement = "Tabuľka"
    }

    show figure.where(kind: image): set figure(supplement: figure_supplement)
    show figure.where(kind: table): set figure(supplement: table_supplement)

    {
      // FRONT PAGE START
      set text(
        font: "TeX Gyre Heros",
        weight: "bold",
      )

      set page(footer: upper[Brno 2025], footer-descent: 28pt)

      let fp_big_text(title, subtitle) = stack(
        spacing: 8pt,
        dir: ttb,
        text(size: 18pt, title),
        text(size: 10.7pt, subtitle),
      )
      let fp_text(title, subtitle) = stack(
        spacing: 8pt,
        dir: ttb,
        text(size: 13pt, title),
        text(size: 10.7pt, subtitle),
      )

      pad(top: 25pt, bottom: 22pt, image(
        "figures/VUT_icon.svg",
        width: 120pt,
        alt: "VUT Logo",
      ))

      stack(
        dir: ttb,
        stack(
          dir: ttb,
          spacing: 21pt,
          upper(fp_big_text(_university(), lang2(_university()))),
          upper(fp_text(_faculty(), lang2(_faculty()))),
          fp_text(
            upper(department),
            upper(department2),
          ),
        ),
        v(1fr),
        fp_big_text(
          upper(title),
          upper(title2),
        ),
        v(1fr),
        stack(
          dir: ttb,
          spacing: 21pt,
          fp_text(
            upper(project_type),
            upper(project_type2),
          ),
          fp_text(
            [
              #upper(_author())
              #h(1fr)
              #author_p #upper[#author_name #author_surname]#if (
                author_a != []
              ) [, #author_a]
            ],
            upper(lang2(_author())),
          ),
          fp_text(
            [
              #upper(_supervisor()) #h(1fr)
              #supervisor_p #upper[#supervisor_name #supervisor_surname]#if (
                supervisor_a != []
              ) [, #supervisor_a]
            ],
            upper(lang2(_supervisor())),
          ),
        ),
        v(8pt),
      )
    } // FRONT PAGE END

    pagebreak()
    v(30pt, weak: false)
    stack(
      spacing: 1fr,
      [
        === #_abstract()

        #abstract
      ],
      lang2[
        === #lang2[#_abstract()]

        #abstract2
      ],
      [
        === #_keywords()

        #keywords
      ],
      lang2[
        === #lang2(_keywords())

        #keywords2
      ],
      [
        === Reference

        #upper(author_surname), #author_name. #text(style: "italic", title). Brno, #date.year().
        #project_type. #_university(), #_faculty(). #_supervisor()
        #supervisor_p #supervisor_name #supervisor_surname#if (
          supervisor_a != []
        ) [, #supervisor_a].
      ],
    )

    pagebreak()
    heading(depth: 2, bookmarked: false, outlined: false, title)
    [=== #_declaration()]
    declaration
    v(1em)
    align(right)[
      #box(width: 25%, repeat([.], gap: 1.2pt)) \
      #author_name #author_surname \
      #date.display("[month repr:long] [day], [year]")
    ]
    v(70pt)
    [=== #_acknowledgments()]
    acknowledgments

    {
      show outline.entry.where(level: 1): set outline.entry(fill: h(1fr))
      show outline.entry.where(level: 1): self => {
        set block(above: 1.5em)
        set text(weight: "bold")
        self
      }
      outline(title: var_heading(_contents()))
    }

    {
      show outline.entry: it => {
        show it.element.caption.at("supplement").text: none
        it
      }
      outline(
        target: figure.where(kind: image),
        title: var_heading(_list_of_figures()),
      )
    }

    set heading(numbering: "1.1", supplement: _chapter())
    set figure(numbering: (..n) => {
      numbering("1.1", counter(heading).get().first(), ..n)
    })
    set math.equation(numbering: (..n) => {
      numbering("1.1", counter(heading).get().first(), ..n)
    })
    include "chapters.typ"

    set heading(numbering: none, supplement: none)
    custom-bibliography(yaml("bibliography.yaml"))

    counter(heading).update(0)
    set heading(numbering: "A.1", supplement: _appendix())
    set figure(numbering: (..n) => {
      numbering("A.1", counter(heading).get().first(), ..n)
    })
    set math.equation(numbering: (..n) => {
      numbering("A.1", counter(heading).get().first(), ..n)
    })
    include "appendices.typ"
  }
}
