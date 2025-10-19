#let bigskip() = block()
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

