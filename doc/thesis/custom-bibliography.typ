#let citations = state("citations", ())
#let custom-cite(entry-name) = {
  citations.update(self => {
    if self.contains(entry-name) { return self }
    (..self, entry-name)
  })
  [[#ref(label(entry-name))]]
}
#let custom-bibliography(yaml-data) = {
  context {
    let _months = if text.lang == "en" {
      (
        "1": [January],
        "2": [February],
        "3": [March],
        "4": [April],
        "5": [May],
        "6": [June],
        "7": [July],
        "8": [August],
        "9": [September],
        "10": [October],
        "11": [November],
        "12": [December],
      )
    } else if text.lang == "cs" {
      (
        "1": [leden],
        "2": [únor],
        "3": [březen],
        "4": [duben],
        "5": [květen],
        "6": [červen],
        "7": [červenec],
        "8": [srpen],
        "9": [září],
        "10": [říjen],
        "11": [listopad],
        "12": [prosinec],
      )
    } else if text.lang == "sk" {
      (
        "1": [január],
        "2": [február],
        "3": [marec],
        "4": [apríl],
        "5": [máj],
        "6": [jún],
        "7": [júl],
        "8": [august],
        "9": [september],
        "10": [október],
        "11": [november],
        "12": [december],
      )
    }
    let _and = () => if text.lang == "en" [and] else [a]
    let _available_at = () => if text.lang == "en" {
      [Available at]
    } else if text.lang == "cs" {
      [Dostupné z]
    } else if text.lang == "sk" {
      [Dostupné z]
    }
    let _bibliography = () => if text.lang == "en" {
      [Bibliography]
    } else if text.lang == "cs" {
      [Literatura]
    } else if text.lang == "sk" {
      [Literatúra]
    }
    let _edition = () => if text.lang == "en" {
      [ed]
    } else if text.lang == "cs" {
      [vyd]
    } else if text.lang == "sk" {
      [vyd]
    }
    let format-order(number) = if text.lang == "en" {
      let rem = calc.rem(number, 10)
      let rem10 = calc.rem(number, 100)
      if rem10 > 10 and rem10 < 20 { return [th] }
      if rem == 1 { return [st] }
      if rem == 2 { return [nd] }
      if rem == 3 { return [rd] }
      [th]
    } else if text.lang == "cs" {
      [.]
    } else if text.lang == "sk" {
      [.]
    }
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
          ) [ et al.] else [ #_and() #format-name(authors.at(1))]
        } else {
          let add_et_al = false
          if authors.len() > 5 {
            authors = authors.slice(0, 5)
            add_et_al = true
          }
          authors.map(author => format-name(author)).join("; ")
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
      if type(date) == int { return [#date] }
      let parts = str.split(date, "-")
      if parts.len() >= 2 {
        _months.at(parts.at(1).trim("0"))
        [ ]
        parts.at(0)
      }
    }
    let format-edition(edition) = {
      if type(edition) == int {
        [#edition]
        format-order(edition)
        [ #_edition()]
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
      if type(sn) == str {
        return [#sn]
      }
    }

    // Format of the citation
    // (one universal, it would be annoying to make separate ones since they're pretty much the same)
    let format-entry(entry, entry_name) = {
      let fields = ()
      let first_field
      if entry.keys().contains("author") {
        first_field = [#format-authors(entry.author) #text(
            style: "italic",
            entry.title,
          )]
      } else {
        first_field = text(style: "italic", entry.title)
      }
      if entry.keys().contains("howpublished") {
        first_field += [ #entry.howpublished]
      }
      fields.push(first_field)
      if entry.keys().contains("version") {
        fields.push(entry.version)
      }
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
      if entry.keys().contains("doi") {
        fields.push[#_available_at(): #link("https://doi.org/" + entry.doi)]
      } else if entry.keys().contains("url") {
        fields.push[#_available_at(): #link(entry.url)]
      }
      if entry.keys().contains("cited") {
        fields.push[[cit.~#entry.cited]]
      }
      show figure: it => []
      [#fields.join(". "). ]
      [#figure(numbering: "1", kind: "citation", supplement: [])[] #label(
          entry_name,
        )]
    }

    heading[#_bibliography()]
    // Actual citation rendering
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
