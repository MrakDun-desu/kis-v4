#{
  import "./custom-bibliography.typ": custom-bibliography

  // config
  let project_type = "Diplomová práce"
  let project_type_en = "Master's Thesis"
  let date = datetime.today()
  // let title = [Modulární systém pro správu studentského klubu s integrací do stávající infrastruktury]
  // let title_en = [Modular Student Club Management System with Integration Into Existing Infrastructure]
  let title = [Název práce]
  let title_en = [Thesis title]
  let department = [Ústav informačních systémů]
  let department_en = [Department of Information Systems]
  let author_p = [Bc.]
  let author_name = [Marek]
  let author_surname = [Dančo]
  let author_a = []
  let supervisor_p = [Ing.]
  let supervisor_name = [Ondřej]
  let supervisor_surname = [Ondryáš]
  let supervisor_a = []
  let keywords = [
    Sem budou zapsána jednotlivá klíčová slova v českém (slovenském) jazyce
    oddělená čárkami.
  ]
  let keywords_en = [
    Sem budou zapsána jednotlivá klíčová slova v anglickém jazyce
    oddělená čárkami.
  ]
  let abstract = [
    Do tohoto odstavce bude zapsán výtah (abstrakt) práce v českém (slovenském) jazyce.
  ]
  let abstract_en = [
    Do tohoto odstavce bude zapsán výtah (abstrakt) práce v českém (slovenském) jazyce.
  ]
  let declaration = [
    Prohlašuji že jsem tuto bakalářskou práci vypracoval samostatně pod vedením pana X...
    Další informace mi poskytli...
    Uvedl jsem všechny literární prameny publikace a další zdroje, ze kterých jsem čerpal.
  ]
  let acknowledgment = [
    V této sekci je možno uvést poděkování vedoucímu práce a těm, kteří poskytli odbornou pomoc
    (externí zadavatel, konzultant apod.).
  ]

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
    lang: "en",
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

  // useful functions
  let cz_text(content) = text(lang: "cz", content)
  let todo(content) = text(red, [[[ #content ]]])
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
        fp_big_text(
          upper[Brno University of Technology],
          upper(cz_text[Vysoké učení technické v Brně]),
        ),
        fp_text(
          upper[Faculty of Information Technology],
          upper(cz_text[Fakulta informačních technologií]),
        ),
        fp_text(
          upper[Department of Information Systems],
          upper(cz_text[Ústav informačních systémů]),
        ),
      ),
      v(1fr),
      fp_big_text(
        upper(title_en),
        upper(title),
      ),
      v(1fr),
      stack(
        dir: ttb,
        spacing: 21pt,
        fp_text(
          upper(project_type_en),
          upper(project_type),
        ),
        fp_text(
          [
            #upper[Author] #h(1fr)
            #author_p #upper[#author_name #author_surname]#if (
              author_a != []
            ) [, #author_a]
          ],
          upper(cz_text[Autor]),
        ),
        fp_text(
          [
            #upper[Supervisor] #h(1fr)
            #supervisor_p #upper[#supervisor_name #supervisor_surname]#if (
              supervisor_a != []
            ) [, #supervisor_a]
          ],
          upper(cz_text[Vedoucí práce]),
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
      === Abstract

      #abstract_en
    ],
    cz_text[
      === #cz_text[Abstrakt]

      #abstract
    ],
    [
      === Keywords

      #keywords_en
    ],
    cz_text[
      === #cz_text[Klíčová slova]

      #keywords
    ],
    [
      === Reference

      #upper(author_surname), #author_name. #text(style: "italic", title_en). Brno, #date.year().
      #project_type_en. Brno University of Technology, Faculty of Information Technology. Supervisor
      #supervisor_p #supervisor_name #supervisor_surname#if (
        supervisor_a != []
      ) [, #supervisor_a]
    ],
  )

  pagebreak()
  heading(depth: 2, bookmarked: false, outlined: false, title_en)
  [=== Declaration]
  declaration
  v(1em)
  align(right)[
    #box(width: 25%, repeat([.], gap: 1.2pt)) \
    #author_name #author_surname \
    #date.display("[month repr:long] [day], [year]")
  ]
  v(70pt)
  [=== Acknowledgements]
  [
    V této sekci je možno uvést poděkování vedoucímu práce a těm, kteří poskytli odbornou pomoc
    (externí zadavatel, konzultant apod.)
  ]

  {
    show outline.entry.where(level: 1): set outline.entry(fill: h(1fr))
    show outline.entry.where(level: 1): self => {
      set block(above: 1.5em)
      set text(weight: "bold")
      self
    }
    outline(title: var_heading[Contents])
  }

  {
    show outline.entry: it => {
      show it.element.caption.at("supplement").text: none
      it
    }
    outline(
      target: figure.where(kind: image),
      title: var_heading[List of Figures],
    )
  }

  set heading(numbering: "1.1", supplement: "Chapter")
  set figure(numbering: (n, ..) => {
    numbering("1.1", counter(heading).get().first(), n)
  })
  include "chapters.typ"

  set heading(numbering: none, supplement: none)
  custom-bibliography(yaml("bibliography.yaml"))

  counter(heading).update(0)
  set heading(numbering: "A.1", supplement: "Appendix")
  set figure(numbering: (..n) => {
    numbering("A.1", counter(heading).get().first(), ..n)
  })
  include "appendices.typ"
}
