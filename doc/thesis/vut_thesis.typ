#import "./custom-bibliography.typ": custom-bibliography

#let vut_thesis(
  lang: "cs",
  lang2: "en",
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
  keywords: [Write keywords in the main language here],
  keywords2: [Write keywords in the secondary language here],
  abstract: [Write abstract in the main language here],
  abstract2: [Write abstract in the secondary language here],
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
    size: 11pt,
  )

  set par(justify: true, first-line-indent: 1.5em, spacing: 0.6em)

  // have to resize the raw text because by default it's smaller than the normal text for some
  // reason
  show raw: set text(font: "Latin Modern Mono", size: 1.2em)
  show link: set text(rgb("#092eab"), font: "Latin Modern Mono")
  show footnote: set text(red)

  // used to replace h1 heading where pagebreak before them can't be used (outlines)
  let var_heading(content) = {
    block(above: 100pt, below: 35pt, content)
  }
  set heading(numbering: none, supplement: none)
  show heading.where(depth: 1): self => {
    pagebreak()
    set text(size: 25pt)
    counter(math.equation).update(0)
    counter(footnote).update(0)
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
  show heading.where(depth: 2): it => {
    set text(size: 18pt)
    set block(above: 30pt, below: 25pt)
    it
  }

  show heading.where(depth: 3): it => {
    set text(size: 15pt)
    set block(below: 15pt)
    it
  }

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

  let dyn_text(lang, content_dict) = {
    text(lang: lang, content_dict.at(lang))
  }

  // fixed words in different languages
  let _university = (
    "cs": [Vysoké učení technické v Brně],
    "sk": [Vysoké učení technické v Brně],
    "en": [Brno University of Technology],
  )
  let _faculty = (
    "cs": [Fakulta informačních technologií],
    "sk": [Fakulta informačních technologií],
    "en": [Faculty of Information Technology],
  )
  let _author = (
    "cs": [Autor],
    "sk": [Autor],
    "en": [Author],
  )
  let _supervisor = (
    "cs": [Vedoucí práce],
    "sk": [Vedúci práce],
    "en": [Supervisor],
  )
  let _abstract = (
    "cs": [Abstrakt],
    "sk": [Abstrakt],
    "en": [Abstract],
  )
  let _keywords = (
    "cs": [Klíčová slova],
    "sk": [Kľúčové slová],
    "en": [Keywords],
  )
  let _declaration = (
    "cs": [Prohlášení],
    "sk": [Prehlásenie],
    "en": [Declaration],
  )
  let _acknowledgments = (
    "cs": [Poděkování],
    "sk": [Poďakovanie],
    "en": [Acknowlegments],
  )
  let _contents = (
    "cs": [Obsah],
    "sk": [Obsah],
    "en": [Contents],
  )
  let _list_of_figures = (
    "cs": [Seznam obrázků],
    "sk": [Zoznam obrázkov],
    "en": [List of Figures],
  )
  let _chapter = (
    "cs": [Kapitola],
    "sk": [Kapitola],
    "en": [Chapter],
  )
  let _appendix = (
    "cs": [Příloha],
    "sk": [Príloha],
    "en": [Appendix],
  )

  // typst doesn't have preset supplements for Slovak for some reason
  let figure_supplement = auto
  let table_supplement = auto
  if lang == "sk" {
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
        upper(fp_big_text(dyn_text(lang, _university), dyn_text(
          lang2,
          _university,
        ))),
        upper(fp_big_text(dyn_text(lang, _faculty), dyn_text(lang2, _faculty))),
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
            #upper(dyn_text(lang, _author))
            #h(1fr)
            #author_p #upper[#author_name #author_surname]#if (
              author_a != []
            ) [, #author_a]
          ],
          upper(dyn_text(lang2, _author)),
        ),
        fp_text(
          [
            #upper(dyn_text(lang, _supervisor))
            #h(1fr)
            #supervisor_p #upper[#supervisor_name #supervisor_surname]#if (
              supervisor_a != []
            ) [, #supervisor_a]
          ],
          upper(dyn_text(lang2, _supervisor)),
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
      === #dyn_text(lang, _abstract)

      #abstract
    ],
    [
      === #dyn_text(lang2, _abstract)

      #abstract2
    ],
    [ === #dyn_text(lang, _keywords)

      #keywords
    ],
    [
      === #dyn_text(lang2, _keywords)

      #keywords2
    ],
    [
      === Reference

      #upper(author_surname), #author_name. #text(style: "italic", title). Brno, #date.year().
      #project_type. #dyn_text(lang, _university), #dyn_text(lang, _faculty). #dyn_text(lang, _supervisor)
      #supervisor_p #supervisor_name #supervisor_surname#if (
        supervisor_a != []
      ) [, #supervisor_a].
    ],
  )

  pagebreak()
  heading(depth: 2, bookmarked: false, outlined: false, title)
  [=== #dyn_text(lang, _declaration)]
  declaration
  v(1em)
  align(right)[
    #box(width: 25%, repeat([.], gap: 1.2pt)) \
    #author_name #author_surname \
    #date.display("[month repr:long] [day], [year]")
  ]
  v(70pt)
  [=== #dyn_text(lang, _acknowledgments)]
  acknowledgments


  counter(page).update(0)
  set page(numbering: "1")

  let in-outline = state("in-outline", false)
  show outline: it => {
    in-outline.update(true)
    it
    in-outline.update(false)
  }

  {
    show outline.entry.where(level: 1): set outline.entry(fill: h(1fr))
    show outline.entry.where(level: 1): it => {
      set block(above: 1.5em)
      set text(weight: "bold")
      it
    }
    outline(title: var_heading(dyn_text(lang, _contents)))
  }

  // short captions instead of the long ones

  {
    show outline.entry: it => {
      show it.element.caption.at("supplement").text: none
      it
    }
    outline(
      target: figure.where(kind: image),
      title: var_heading(dyn_text(lang, _list_of_figures)),
    )
  }

  set heading(numbering: "1.1", supplement: dyn_text(lang, _chapter))
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
  set heading(numbering: "A.1", supplement: dyn_text(lang, _appendix))
  set figure(numbering: (..n) => {
    numbering("A.1", counter(heading).get().first(), ..n)
  })
  set math.equation(numbering: (..n) => {
    numbering("A.1", counter(heading).get().first(), ..n)
  })
  include "appendices.typ"
}
