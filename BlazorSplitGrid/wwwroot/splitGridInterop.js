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
            element: document.getElementById(x.id)
        }
    });

    options.rowGutters = rows.map(x => {
        return {
            track: x.track,
            element: document.getElementById(x.id)
        }
    });
    
    console.log(options);

    const split = window.Split(options);

    split.addColumnGutterById = (id, track) =>
    {
        const style = document.getElementById(splitGridStyleId);
        if (!style.innerHTML.includes(`.split-grid-gutter-column-${track}`)) {
            style.innerHTML += `\n.split-grid-gutter-column-${track} { grid-column: ${track + 1}; }`;
        }

        split.addColumnGutter(document.getElementById(id), track);
    };

    split.addRowGutterById = (id, track) =>
    {
        const style = document.getElementById(splitGridStyleId);
        if (!style.innerHTML.includes(`.split-grid-gutter-row-${track}`)) {
            style.innerHTML += `\n.split-grid-gutter-row-${track} { grid-row: ${track + 1}; }`;
        }

        split.addRowGutter(document.getElementById(id), track);
    };

    split.removeColumnGutterById = (id, track, immediate = true) =>
    {
        split.removeColumnGutter(document.getElementById(id), track, immediate);
    };

    split.removeRowGutterById = (id, track, immediate = true) =>
    {
        split.removeRowGutter(document.getElementById(id), track, immediate);
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
    if(rows.length > 0) {
        let template = "1fr ";
        for (let i = 0 ; i < rows.length; i++) {
            template += "10px 1fr ";
        }

        cssBuilder.push(`\tgrid-template-rows: ${template};`);
    }

    if(columns.length > 0) {
        let template = "1fr ";
        for (let i = 0 ; i < columns.length; i++) {
            template += "10px 1fr ";
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
