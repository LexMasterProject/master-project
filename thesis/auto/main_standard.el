(TeX-add-style-hook
 "main_standard"
 (lambda ()
   (TeX-add-to-alist 'LaTeX-provided-class-options
                     '(("book" "11pt" "oneside")))
   (TeX-add-to-alist 'LaTeX-provided-package-options
                     '(("geometry" "margin=1.2in") ("appendix" "toc" "page") ("hyphenat" "none")))
   (TeX-run-style-hooks
    "latex2e"
    "chapters/abstract"
    "chapters/chapter1"
    "chapters/chapter2"
    "chapters/chapter3"
    "chapters/chapter4"
    "chapters/chapter5"
    "chapters/appendixA"
    "chapters/appendixB"
    "book"
    "bk11"
    "geometry"
    "setspace"
    "appendix"
    "hyphenat"
    "graphicx"
    "color"
    "url"
    "amsmath"
    "listings"
    "algorithm"
    "algorithmic"
    "subfig"
    "hyperref"
    "dirtytalk"
    "perpage")
   (LaTeX-add-bibliographies
    "mybibliography")))

