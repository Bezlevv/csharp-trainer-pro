(function () {
  "use strict";

  var DATA = window.COURSE_DATA || {};
  var MODULES = window.COURSE_MODULES || [];
  var TABS = [
    { id: "teoria", label: "Теория" },
    { id: "praktika", label: "Практика" },
    { id: "razbor", label: "Разбор кода" }
  ];
  var state = { index: 0, tab: "teoria" };

  function $(sel) { return document.querySelector(sel); }

  function renderMenu() {
    var m = $("#menu");
    m.innerHTML = "";
    MODULES.forEach(function (x, i) {
      var b = document.createElement("button");
      b.textContent = x.title;
      b.dataset.index = i;
      b.onclick = function () { openModule(i); };
      m.appendChild(b);
    });
  }

  function updateActive() {
    var buttons = document.querySelectorAll("#menu button");
    for (var i = 0; i < buttons.length; i++) {
      buttons[i].classList.toggle("active", Number(buttons[i].dataset.index) === state.index);
    }
  }

  function renderView() {
    var mod = MODULES[state.index];
    var tab = TABS[0];
    for (var t = 0; t < TABS.length; t++) {
      if (TABS[t].id === state.tab) { tab = TABS[t]; break; }
    }
    var html = (DATA[mod.slug] || {})[tab.id] || '<div class="error-box">Материал отсутствует</div>';

    var tabsHtml = '<div class="tabs">';
    for (var i = 0; i < TABS.length; i++) {
      tabsHtml += '<button data-tab="' + TABS[i].id + '" class="' +
        (TABS[i].id === state.tab ? "active" : "") + '">' + TABS[i].label + "</button>";
    }
    tabsHtml += "</div>";

    var prev = state.index > 0
      ? '<a href="#" data-nav="' + (state.index - 1) + '">← ' + MODULES[state.index - 1].title + "</a>"
      : "<span></span>";
    var next = state.index < MODULES.length - 1
      ? '<a href="#" data-nav="' + (state.index + 1) + '">' + MODULES[state.index + 1].title + " →</a>"
      : "<span></span>";

    $("#view").innerHTML = tabsHtml +
      '<div class="content">' + html + "</div>" +
      '<div class="nav-bottom">' + prev + next + "</div>";

    if (window.hljs) {
      var codes = document.querySelectorAll("#view pre code");
      for (var c = 0; c < codes.length; c++) {
        try { hljs.highlightElement(codes[c]); } catch (e) {}
      }
    }

    var tabButtons = document.querySelectorAll("#view .tabs button");
    for (var tb = 0; tb < tabButtons.length; tb++) {
      tabButtons[tb].onclick = function () { state.tab = this.dataset.tab; renderView(); };
    }
    var navLinks = document.querySelectorAll("#view [data-nav]");
    for (var n = 0; n < navLinks.length; n++) {
      navLinks[n].onclick = function (e) { e.preventDefault(); openModule(Number(this.dataset.nav)); };
    }
  }

  function openModule(i) {
    state.index = i;
    state.tab = "teoria";
    updateActive();
    renderView();
  }

  window.openModule = openModule;

  document.addEventListener("DOMContentLoaded", function () {
    renderMenu();
    openModule(0);
  });
})();
