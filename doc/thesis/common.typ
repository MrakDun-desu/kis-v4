#let bigskip() = block()
#let todo(content) = text(red, [*[ [ #content ] ]*])
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
#let flex-caption(short: none, long: none) = context if state(
  "in-outline",
  false,
).get() { short }
else {
long
}
