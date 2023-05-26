import './splitGrid/split-grid.min.js';

const splitGridLinkHref = "_content/BlazorSplitGrid/splitGrid/splitGrid.css";
const splitGridLinkId = "split-grid-link";
const splitGridStyleId = "split-grid-style";

export function initSplitGrid(rows, columns, options, interopReference) {
    BuildStylesheetLink();
    BuildStyles(rows, columns);

    if(options.hasOnDrag) {
        options.onDrag = function(direction, track, gridTemplateStyle) {
            interopReference.invokeMethod("OnDragFired", direction, track, gridTemplateStyle);
        }
    }

    if(options.hasOnDragStart) {
        options.onDragStart = function (direction, track) {
            interopReference.invokeMethod("OnDragStartFired", direction, track);
        }
    }

    if(options.hasOnDragStop) {
        options.onDragEnd = function (direction, track) {
            interopReference.invokeMethod("OnDragEndFired", direction, track);
        }
    }

    options.columnGutters = columns.map(x => {
        return {
            track: x.track,
            element: document.querySelector(`.${x.id}`)
        }
    });

    options.rowGutters = rows.map(x => {
        return {
            track: x.track,
            element: document.querySelector(`.${x.id}`)
        }
    });

    const split = window.Split(options);

    split.addColumnGutterByQuerySelector = (selector, track) =>
    {
        const style = document.getElementById(splitGridStyleId);
        if (!style.innerHTML.includes(`.split-grid-gutter-column-${track}`)) {
            style.innerHTML += `\n.split-grid-gutter-column-${track} { grid-column: ${track + 1}; }`;
        }

        split.addColumnGutter(document.querySelector(selector), track);
    };

    split.addRowGutterByQuerySelector = (selector, track) =>
    {
        const style = document.getElementById(splitGridStyleId);
        if (!style.innerHTML.includes(`.split-grid-gutter-row-${track}`)) {
            style.innerHTML += `\n.split-grid-gutter-row-${track} { grid-row: ${track + 1}; }`;
        }

        split.addRowGutter(document.querySelector(selector), track);
    };

    split.removeColumnGutterByQuerySelector = (selector, track, immediate = true) =>
    {
        split.removeColumnGutter(document.querySelector(selector), track, immediate);
    };

    split.removeRowGutterByQuerySelector = (selector, track, immediate = true) =>
    {
        split.removeRowGutter(document.querySelector(selector), track, immediate);
    };

    return split;
}

function BuildStylesheetLink()  {
    if (document.getElementById(splitGridLinkId)) {
        return;
    }

    const link = document.createElement('link');
    link.id = splitGridLinkId;
    link.href = splitGridLinkHref;
    link.rel = "stylesheet";
    document.head.appendChild(link);
}

function BuildStyles(rows, columns) {
    let style = document.getElementById(splitGridStyleId);

    if (!style) {
        style = document.createElement('style');
        style.id = splitGridStyleId;
        document.head.appendChild(style);
    }
    
    const cssBuilder = [];

    cssBuilder.push('.split-grid {');
    cssBuilder.push('\tdisplay: grid;');
    cssBuilder.push('\theight: 100%;');
    if(rows.length > 0) {
        let template = "1fr ";
        for (let i = 0 ; i < rows.length; i++) {
            template += `${rows[i].size}px 1fr `;
        }

        cssBuilder.push(`\tgrid-template-rows: ${template};`);
    }

    if(columns.length > 0) {
        let template = "1fr ";
        for (let i = 0 ; i < columns.length; i++) {
            template += `${columns[i].size}px 1fr `;
        }

        cssBuilder.push(`\tgrid-template-columns: ${template};`);
    }
    cssBuilder.push('}');

    for (let row of rows) {
        const track = row.track;
        cssBuilder.push(`.split-grid-gutter-row-${track} { grid-row: ${track + 1}; }`);
    }

    for (let column of columns) {
        const track = column.track;
        cssBuilder.push(`.split-grid-gutter-column-${track} { grid-column: ${track + 1}; }`);
    }

    style.innerHTML = cssBuilder.join('\n');
}
