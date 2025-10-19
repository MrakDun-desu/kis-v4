#let bigskip() = block()
#let todo(content) = text(red, [[[ #content ]]])
#let cs(content) = text(lang: "cs", content)
#let mini_heading(content) = block(
  above: 20pt,
  below: 14pt,
  par(
    text(
      size: 12pt,
      weight: "bold",
      content,
    ),
  ),
)

