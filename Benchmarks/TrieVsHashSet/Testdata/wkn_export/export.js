const jsdom = require("jsdom");

const { JSDOM } = jsdom;

const ONVISTA_BASE_URL = 'https://www.onvista.de';
const ONVISTA_ALL_COUNTRIES_URL = 'https://www.onvista.de/aktien/aktien-laender';
const COUNTRY_SELECTOR = 'a[href*="aktien/aktien-laender/"]';

const WKN_SELECTOR = '#finderResults tbody tr td:nth-child(2)';
const NEXT_PAGE_SELECTOR = '[aria-label="Nächste Seite"]';

function parseHtml(htmlText) {
  return new JSDOM(htmlText);
}

function* extractCountryLinks(htmlDocument) {
  const countryElements = htmlDocument.window.document.querySelectorAll(COUNTRY_SELECTOR);
  for (let i = 0; i < countryElements.length; ++i) {
    const linkText = countryElements[i].href;

    yield ONVISTA_BASE_URL + linkText;
  }
}

function* extractWKNs(htmlDocument) {
  const wknCells = htmlDocument.window.document.querySelectorAll(WKN_SELECTOR);
  for (let i = 0; i < wknCells.length; ++i) {
    const wkn = wknCells[i].textContent;

    if (wkn.length !== 6) {
      continue;
    }

    yield wkn;
  }
}

function extractHasNextPage(htmlDocument) {
  const nextButtonElement = htmlDocument.window.document.querySelector(NEXT_PAGE_SELECTOR);
  return nextButtonElement !== null && !nextButtonElement.disabled;
}

function loadPage(countryUrl, pageIndex) {
  return fetch(`${countryUrl}?page=${pageIndex}`)
    .then(response => response.text())
    .then(parseHtml)
    .then(htmlDocument => ({
      wknIterator: extractWKNs(htmlDocument),
      hasNextPage: extractHasNextPage(htmlDocument),
    }))
    .then(result => {
      for (const wkn of result.wknIterator) {
        console.log(wkn);
      }

      if (!result.hasNextPage) {
        return Promise.resolve();
      }

      return loadPage(countryUrl, pageIndex + 1);
    })

}

function loadAllCountries() {
  return fetch(ONVISTA_ALL_COUNTRIES_URL)
    .then(response => response.text())
    .then(parseHtml)
    .then(extractCountryLinks);
}

loadAllCountries()
  .then(countryLinks => {
    const tasks = [];

    for (const countryLink of countryLinks) {
     tasks.push(loadPage(countryLink, 0));
    }

    return Promise.all(tasks);
  })
  .then(() => console.log('finished'));