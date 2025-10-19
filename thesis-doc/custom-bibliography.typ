#let citations = state("citations", ())
#let custom-cite(entry-name) = {
  citations.update(self => {
    if self.contains(entry-name) { return self }
    (..self, entry-name)
  })
  [[#ref(label(entry-name))]]
}
#let custom-bibliography(yaml-data, show-unused: false) = {
  // Formatting helper functions to be used in the formats themselves
  let format-name(name) = {
    if name == "others" {
      return [et al]
    }
    let parts = str.split(name, ", ")
    if parts.len() == 1 {
      smallcaps(parts.at(0))
    } else {
      smallcaps(parts.at(0))
      [, ]
      let removed-chars = "áčďěéíĺľňóřšťúý"
      str.replace(parts.at(1), regex("[a-z" + removed-chars + "]+"), ".")
    }
  }
  let format-authors(authors) = {
    if type(authors) == array {
      if authors.len() == 2 {
        [#format-name(authors.at(0))]
        if (
          authors.last() == "others"
        ) [ et al.] else [ and #format-name(authors.at(1))]
      } else {
        let add_et_al = false
        if authors.len() > 5 {
          authors = authors.slice(0, 5)
          add_et_al = true
        }
        authors.map(author => format-name(author)).join(", ")
        if add_et_al [ et al.]
        if authors.last() == "others" [.]
      }
    } else {
      format-name(authors)
      [ ]
    }
  }
  let format-publisher(publisher) = {
    if type(publisher) == dictionary {
      let output = ()
      if publisher.keys().contains("location") {
        output.push(publisher.location)
      }
      if publisher.keys().contains("name") { output.push(publisher.name) }
      return output.join(": ")
    } else if type(publisher) == str {
      [#publisher]
    }
  }
  let format-date(date) = {
    let months = (
      "1": [january],
      "2": [february],
      "3": [march],
      "4": [april],
      "5": [may],
      "6": [june],
      "7": [july],
      "8": [august],
      "9": [september],
      "10": [october],
      "11": [november],
      "12": [december],
    )
    if type(date) == int { return [#date] }
    let parts = str.split(date, "-")
    if parts.len() == 2 {
      months.at(parts.at(1))
      [ ]
      parts.at(0)
    }
  }
  let format-edition(edition) = {
    let format-order(number) = {
      let rem = calc.rem(edition, 10)
      let rem10 = calc.rem(edition, 100)
      if rem10 > 10 and rem10 < 20 { return [th] }
      if rem == 1 { return [st] }
      if rem == 2 { return [nd] }
      if rem == 3 { return [rd] }
      [th]
    }
    if type(edition) == int {
      [#edition]
      format-order(edition)
      [ ed]
    }
  }
  let format-serial-number(sn) = {
    if type(sn) == dictionary {
      if sn.keys().contains("isbn") {
        return [ISBN #sn.isbn]
      }
      if sn.keys().contains("issn") {
        return [ISSN #sn.issn]
      }
    }
  }

  // Format of the citation
  // (one universal, it would be annoying to make separate ones since they're pretty mych the same)
  let format-entry(entry, entry_name) = {
    let fields = ()
    let first_field = [#format-authors(entry.author) #text(
        style: "italic",
        entry.title,
      )]
    if entry.keys().contains("note") {
      first_field += [ #entry.note]
    }
    fields.push(first_field)
    if entry.keys().contains("edition") {
      fields.push(format-edition(entry.edition))
    }
    let publisher_and_date = ()
    if entry.keys().contains("publisher") {
      publisher_and_date.push(format-publisher(entry.publisher))
    }
    if entry.keys().contains("date") {
      publisher_and_date.push(format-date(entry.date))
    }
    fields.push(publisher_and_date.join(", "))
    if entry.keys().contains("genre") { fields.push(entry.genre) }
    if entry.keys().contains("school") { fields.push(entry.school) }
    if entry.keys().contains("serial-number") {
      fields.push(format-serial-number(entry.serial-number))
    }
    if entry.keys().contains("url") {
      fields.push[Available at: #link(entry.url)]
    }
    if entry.keys().contains("cited") {
      fields.push[[cit. #entry.cited]]
    }
    show figure: it => []
    fields.join(". ")
    [. ]
    [#figure(numbering: "1", kind: "citation", supplement: [])[] #label(
        entry_name,
      )]
  }

  heading[Bibliography]
  // Actual citation rendering
  context {
    let citationsVal = citations.final()
    if citationsVal == none { return }
    enum(
      numbering: "[1]",
      indent: 0em,
      body-indent: 1em,
      spacing: 1.5em,
      ..citationsVal.map(elem => { format-entry(yaml-data.at(elem), elem) }),
    )
  }
}
}
