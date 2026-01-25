DRAWIO_SOURCES = $(wildcard figures/*.drawio)
DRAWIO_FIGURES = $(DRAWIO_SOURCES:.drawio=.pdf)

all: thesis.pdf

figures/%.pdf: figures/%.drawio
	drawio -x --crop $<

thesis.pdf: main.typ $(DRAWIO_FIGURES)
	typst c main.typ $@

clean:
	rm $(DRAWIO_FIGURES)
	rm *.pdf

.PHONY: all clean
