import './splitGrid/split-grid.min.js';

const splitGridLinkHref = "_content/BlazorSplitGrid/splitGrid/splitGrid.css";
const splitGridLinkId = "split-grid-link";
const splitGridStyleId = "split-grid-style";

export function initSplitGrid(rows, columns, options, interopReference) {
    BuildStylesheetLink();
    BuildStyles(options.css);

    if (options.hasOnDrag) {
        options.onDrag = function(direction, track, gridTemplateStyle) {
            interopReference.invokeMethodAsync("OnDragFired", direction, track, gridTemplateStyle);
        }
    }

    if (options.hasOnDragStart) {
        options.onDragStart = function (direction, track) {
            const templateName = direction === "column" ? "grid-template-columns" : "grid-template-rows";
            const element = document.querySelector(".split-grid");
            const gridTemplateStyle = element.style[templateName];
            interopReference.invokeMethodAsync("OnDragStartFired", direction, track, gridTemplateStyle);
        }
    }

    if (options.hasOnDragStop) {
        options.onDragEnd = function (direction, track) {
            const templateName = direction === "column" ? "grid-template-columns" : "grid-template-rows";
            const element = document.querySelector(".split-grid");
            const gridTemplateStyle = element.style[templateName];
            interopReference.invokeMethodAsync("OnDragEndFired", direction, track, gridTemplateStyle);
        }
    }

    options.columnGutters = columns.map(x => {
        return {
            track: x.number,
            element: document.querySelector(`.${x.id}`)
        }
    });

    options.rowGutters = rows.map(x => {
        return {
            track: x.number,
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

    split.setGridSizes = (selector, templateName, sizes) =>
    {
        document.querySelector(selector).style[templateName] = sizes
    };

    split.getGridSizes = (selector, templateName) =>
    {
        const element = document.querySelector(selector);
        const styleElement = element.style[templateName];
        return styleElement.toString();
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

function BuildStyles(css) {
    let style = document.getElementById(splitGridStyleId);

    if (!style) {
        style = document.createElement('style');
        style.id = splitGridStyleId;
        document.head.appendChild(style);
    }

    style.innerHTML = css;
}

export function createResizeObserver(methodName, dotNetRef) {
    return new ResizeObserver(entries => {
        entries.forEach(entry => {
            const width = entry.contentRect.width;
            const height = entry.contentRect.height;
            dotNetRef.invokeMethodAsync(methodName, width, height).catch(err => console.error(err));
        });
    });
}
