MMD_SOURCES = $(wildcard figures/*.mmd)
FIGURES = $(MMD_SOURCES:.mmd=.svg)

all: thesis.pdf

figures/%.svg: figures/%.mmd
	mmdc -i $< -o $@

thesis.pdf: main.typ $(FIGURES)
	typst c main.typ $@

clean:
	rm *.pdf
