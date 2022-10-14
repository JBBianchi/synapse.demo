import Knob from "https://cdn.skypack.dev/svg-knob@1.15.0";

export function createKnob(knobContainer, min, max, value, icon) {
    //const knobContainer = document.getElementById(elementId);
    const elementId = knobContainer.id;
    /* clean/reset the DOM element */
    const clone = knobContainer.cloneNode(false);
    const knobElement = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
    knobElement.classList.add('knob-element');
    clone.appendChild(knobElement);
    knobContainer.replaceWith(clone);
    /* init the knob */
    const accentColor = getComputedStyle(document.body).getPropertyValue('--color-accent');
    const config = {
        value_min: min,
        value_max: max,
        track_width: 10,
        track_color: accentColor
    };
    const knob = new Knob(`#${elementId} .knob-element`, config);
    knob.value = value;
    knobElement.addEventListener('change', (e) => {
        knob.value = value;
    });
    if (icon) {
        const knobValue = knobElement.querySelector('.knob-value');
        knobElement.removeChild(knobValue);
        const iconHtml = `<span class="material-symbols-outlined active">${icon}</span>`;
        const dummy = document.createElement('template');
        dummy.innerHTML = iconHtml;
        const iconElement = dummy.content.firstChild;
        clone.appendChild(iconElement);
    }
    return knob;
};