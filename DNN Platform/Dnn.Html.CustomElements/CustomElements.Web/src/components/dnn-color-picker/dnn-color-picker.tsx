/** Reusable DNN UI component to pick a color
 * @copyright Copyright (c) .NET Foundation. All rights reserved.
 * @license MIT 
 */

import { Component, h, State, Element, Prop } from "@stencil/core";
import { ColorInfo } from '../../utils/ColorInfo';

const enum DisplayMode {
    rgb,
    hex,
    hsl
}

/** Color Picker for Dnn */
@Component({
    tag: 'dnn-color-picker',
    styleUrl: 'dnn-color-picker.scss',
    shadow: true
})
export class DnnColorPicker {

    @Prop() colorBoxHeight: string = "50%";

    @State() color: ColorInfo;
    @State() displayMode: DisplayMode;
    @State() componentWidth: number;

    @Element() el: HTMLElement;

    private saturationLightnessBox?: HTMLDivElement;
    private hueRange?: HTMLDivElement;
    private switchDisplayModeLabel?: String;
    private breakPoint = 330;

    componentWillLoad() {
        this.color = new ColorInfo();
        this.displayMode = DisplayMode.rgb;
        this.switchDisplayModeLabel = "swith to hexadecimal entry mode";
        this.updateComponentWidth();
        window.addEventListener('resize', this.updateComponentWidth);
    }

    componentWillUpdate() {
        this.updateComponentWidth();
    }

    conponentWillUnload() {
        window.removeEventListener('resize', this.updateComponentWidth);
    }

    updateComponentWidth = () => {
        const container = this.el.shadowRoot.querySelector('.dnn-color-entry');
        if (!container) { return }
        const newWidth = container.getBoundingClientRect().width;
        if (this.componentWidth !== newWidth){
            this.componentWidth = newWidth;
        }
    }

    getHex() {
       return this.getDoublet(this.color.red) + this.getDoublet(this.color.green) + this.getDoublet(this.color.blue);
    }

    getDoublet(value: number){
        const valueString = value.toString(16).toUpperCase();
        if (valueString.length === 1){
            return '0' + valueString;
        }
        return valueString;
    }

    handleSaturationLightnessMouseDown = (e) => {
        e.preventDefault();
        this.handleDragLightnessSaturation(e);
        window.addEventListener('mousemove', this.handleDragLightnessSaturation);
        window.addEventListener('mouseup', this.handleSaturationLightnessMouseUp);
    }

    handleDragLightnessSaturation = (e) => {
        const rect = this.saturationLightnessBox.getBoundingClientRect();        

        let x = e.clientX - rect.left;        
        if (x < 0) { x = 0}
        if (x > rect.width) { x = rect.width}
        x = x/rect.width;

        let y = e.clientY - rect.top;
        if (y < 0) { y = 0}
        if (y > rect.height) { y = rect.height}
        y = 1 - (y/rect.height);

        const newColor = new ColorInfo();
        newColor.hue = this.color.hue;
        newColor.saturation = x;
        newColor.lightness = y;
        this.color = newColor;
    }

    handleSaturationLightnessMouseUp = () => {
        window.removeEventListener('mousemove', this.handleDragLightnessSaturation);
        window.removeEventListener('mouseup', this.handleSaturationLightnessMouseUp);
    }

    handleHueMouseDown = (e) => {
        e.preventDefault();
        this.handleDragHue(e);
        window.addEventListener('mousemove', this.handleDragHue);
        window.addEventListener('mouseup', this.handleHueMouseUp);        
    }

    handleHueMouseUp = () => {
        window.removeEventListener('mousemove', this.handleDragHue);
        window.removeEventListener('mouseup', this.handleHueMouseUp); 
    }

    handleDragHue = (e) => {
        const rect = this.hueRange.getBoundingClientRect();        

        let x = e.clientX - rect.left;
        if (x < 0) { x = 0}
        if (x > rect.width) { x = rect.width}
        x = x/rect.width*360;        

        const newColor = new ColorInfo();
        newColor.hue = x;
        newColor.saturation = this.color.saturation;
        newColor.lightness = this.color.lightness;
        this.color = newColor;
    }

    handleComponentValueChange = (e, channel) => {
        let value = parseInt(e.detail);
        if (isNaN(value)) { return }
        const newColor = new ColorInfo();
        if (value){
            if (value < 0) { value = 0; }
            if (value > 255) { value = 255; }
        }
        let r = this.color.red;
        let g = this.color.green;
        let b = this.color.blue;
        switch (channel) {
            case 'red':
                r = value;
                break;
            case 'green':
                g = value;
                break;
            case 'blue':
                b = value;
                break;
            default:
                break;
        }
        newColor.green = g;
        newColor.red = r;
        newColor.blue = b;
        this.color = newColor;
    }

    handleHSLChange = (e, component) => {    
        let value = parseInt(e.target.value);
        if (isNaN(value)) {return}
        const newColor = new ColorInfo();
        if (value) {            
            let h = this.color.hue;
            let s = this.color.saturation;
            let l = this.color.lightness;
            switch (component) {
                case "hue":
                    if (value < 0) { value = 0}
                    if (value > 359) { value = 0}
                    h = value;
                    break;
                case "saturation":
                    if (value < 0) { value = 0}
                    if (value > 100) { value = 100}
                    s = value / 100;
                    break;
                case "lightness":
                    if (value < 0) { value = 0}
                    if (value > 100) { value = 100}
                    l = value / 100;
                    break;            
                default:
                    break;                
            }
            newColor.hue = h;
            newColor.saturation = s;
            newColor.lightness = l;
            this.color = newColor;
        }
    }

    handleHexChange(e){
        let value: string = e.target.value;
        const newColor = new ColorInfo();
        if (value.match(/^(?:[\da-f]{3}|[\da-f]{6})$/i)) {
            if (value.length === 3){
                let expanded = value[0] + value[0] + value[1] + value[1] + value[2] + value [2];
                value = expanded;
            }
            newColor.red = parseInt(value.substr(0,2), 16);
            newColor.green = parseInt(value.substr(2,2), 16);
            newColor.blue = parseInt(value.substr(4,2), 16);
        }
        else{
            newColor.red = this.color.red;
            newColor.green = this.color.green;
            newColor.blue = this.color.blue;
        }
        this.color = newColor;
    }

    handleSwitchDisplayModeClick(){
        switch (this.displayMode) {
            case DisplayMode.rgb:
                this.displayMode = DisplayMode.hex;
                this.switchDisplayModeLabel = "switch to hue, saturation, lightness entry mode";
                break;
            case DisplayMode.hex:
                this.displayMode = DisplayMode.hsl;
                this.switchDisplayModeLabel = "switch to red, green, blue entry mode";
                break;
            case DisplayMode.hsl:
                this.displayMode = DisplayMode.rgb;
                this.switchDisplayModeLabel = "switch to hexadecimal entry mode"
            default:
                break;
        }
    }

    handleSaturationLightnessKeyDown = (e) => {        
        let newColor = new ColorInfo();
        newColor.hue = this.color.hue;
        newColor.saturation = this.color.saturation;
        newColor.lightness = this.color.lightness;

        let value = 0.01;
        if (e.shiftKey) { value = 0.1 ;}

        switch (e.key) {
            case "ArrowUp":
                newColor.lightness += value;
                break;
            case "ArrowDown":
                newColor.lightness -= value;
                break;
            case "ArrowLeft":
                newColor.saturation -= value;
                break;
            case "ArrowRight":
                newColor.saturation += value;
            default:
                break;                
        }
        this.color = newColor;
    }

    handleHueKeyDown = (e) => {
        let newColor = new ColorInfo();
        newColor.hue = this.color.hue;
        newColor.saturation = this.color.saturation;
        newColor.lightness = this.color.lightness;

        let value = 1;
        if (e.shiftKey) { value = 10}

        switch (e.key) {
            case "ArrowLeft":
                newColor.hue -= value;
                break;
            case "ArrowRight":
                newColor.hue += value;
            default:
                break;
        }
        this.color = newColor;
    }

    handleCopyValue = () => {
        navigator.clipboard.writeText("#" + this.getHex());
    }

    render() {
        const hue = this.color.hue;
        const saturation = this.color.saturation;
        const lightness = this.color.lightness;
        const red = this.color.red;
        const green = this.color.green;
        const blue = this.color.blue;

        this.el.style.setProperty('--color-box-height', this.colorBoxHeight.toString());

        return (
            <div class="dnn-color-picker">
                <div class="dnn-color-sliders">
                    <div class="dnn-color-s-b" ref={(element) => this.saturationLightnessBox = element as HTMLDivElement}
                        style={{backgroundColor: `hsl(${hue},100%,50%)`}}
                        onMouseDown={this.handleSaturationLightnessMouseDown.bind(this)}
                    >
                        <button class="dnn-s-b-picker"
                            aria-label="Press up or down to adjust lightness, left or right to adjust saturation, hold shift to move by 10%"
                            role="slider"
                            aria-valuemin="0"
                            aria-valuemax="100"
                            aria-valuetext={`Saturation: ${Math.round(this.color.saturation*100)}%, Lightness: ${Math.round(this.color.lightness*100)}%`}
                            style={{
                                left: Math.round(saturation * 100)  + "%",
                                bottom: Math.round(lightness * 100)  + "%"
                            }}
                            onKeyDown={(e) => this.handleSaturationLightnessKeyDown(e)}
                        />
                    </div>
                    <div class="dnn-color-bar">
                        <div class="dnn-color-result" style={{backgroundColor: '#' + this.getHex()}}></div>
                        <div class="dnn-color-hue"
                            ref={(element) => this.hueRange = element as HTMLDivElement}
                            onMouseDown={this.handleHueMouseDown.bind(this)}
                        >
                            <button class="dnn-hue-picker"
                                aria-label="Press left or right to adjust hue, hold shift to move by 10 degrees"
                                role="slider"
                                aria-valuemin="0"
                                aria-valuemax="359"
                                aria-valuenow={Math.round(hue)}
                                style={{left: (hue/359*100).toString() + "%"}}
                                onKeyDown={(e) => this.handleHueKeyDown(e)}
                            />
                        </div>
                    </div>
                </div>
                <div class="dnn-color-entry">
                    <div class="dnn-color-fields">
                        <div class={this.displayMode === DisplayMode.rgb ? "dnn-color-field-group visible" : "dnn-color-field-group"}
                            style={{flexDirection: this.componentWidth < this.breakPoint ? 'column' : 'row'}}
                        >
                            <div class="dnn-color-field">
                                <label>R</label>
                                <dnn-input-number value={red} min={0} max={255} use-wheel={true} aria-label="red value" 
                                    onChange={(e) => this.handleComponentValueChange(e, 'red')}
                                />                                
                            </div>
                            <div class="dnn-color-field">
                                <label>G</label>
                                <dnn-input-number value={green} min={0} max={255} use-wheel={true} aria-label="green value" 
                                    onChange={(e) => this.handleComponentValueChange(e, 'green')}
                                />
                            </div>
                            <div class="dnn-color-field">
                                <label>B</label>
                                <dnn-input-number value={blue} min={0} max={255} use-wheel={true} aria-label="blue value" 
                                    onChange={(e) => this.handleComponentValueChange(e, 'blue')}
                                />
                            </div>                        
                        </div>
                        <div class={this.displayMode === DisplayMode.hex ? "dnn-color-field-group visible" : "dnn-color-field-group"}
                            style={{flexDirection: this.componentWidth < this.breakPoint ? 'column' : 'row'}}
                        >
                            <div class="dnn-color-field">
                                <label>H</label>
                                <dnn-input-number value={Math.round(hue)} min={0} max={359} aria-label="Hue"
                                    onChange = {(e) => this.handleHSLChange(e, 'hue')} />
                            </div>
                            <div class="dnn-color-field">
                                <label>S</label>
                                <dnn-input-number min={0} max={100} step={1} value={Math.round(saturation*100)} aria-label="Saturation"
                                    onChange={(e) => this.handleHSLChange(e, 'saturation')}
                                />
                            </div>
                            <div class="dnn-color-field">
                                <label>L</label>
                                <dnn-input-number min={0} max={100} step={1} value={Math.round(lightness*100)} aria-label="Lightness"
                                    onChange={(e) => this.handleHSLChange(e, 'lightness')}
                                />
                            </div>
                        </div>
                        <div class={this.displayMode === DisplayMode.hsl ? "dnn-color-field-group visible" : "dnn-color-field-group"}>
                            <div class="dnn-color-field">
                                <label>HEX</label>
                                <div class="hex-input">
                                    <input type="text" size={10} aria-label="Hexadecimal value"
                                        value={this.getHex()}
                                        onChange={(e) => this.handleHexChange(e)}
                                    />
                                    <button class="copy" aria-label="copy value" onClick={() => this.handleCopyValue()}>
                                        <svg aria-hidden="true" focusable="false" data-prefix="fas" data-icon="copy" class="svg-inline--fa fa-copy fa-w-14" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512"><path fill="currentColor" d="M320 448v40c0 13.255-10.745 24-24 24H24c-13.255 0-24-10.745-24-24V120c0-13.255 10.745-24 24-24h72v296c0 30.879 25.121 56 56 56h168zm0-344V0H152c-13.255 0-24 10.745-24 24v368c0 13.255 10.745 24 24 24h272c13.255 0 24-10.745 24-24V128H344c-13.2 0-24-10.8-24-24zm120.971-31.029L375.029 7.029A24 24 0 0 0 358.059 0H352v96h96v-6.059a24 24 0 0 0-7.029-16.97z"></path></svg>
                                    </button>
                                </div>
                            </div>
                        </div>                    
                    </div>
                    <div class="dnn-color-mode-switch">
                        <button id="rgb-switch" onClick={this.handleSwitchDisplayModeClick.bind(this)} aria-label={this.switchDisplayModeLabel}>
                            <svg xmlns="http://www.w3.org/2000/svg" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="retweet" class="svg-inline--fa fa-retweet fa-w-20" role="img" viewBox="0 0 640 512"><path fill="currentColor" d="M629.657 343.598L528.971 444.284c-9.373 9.372-24.568 9.372-33.941 0L394.343 343.598c-9.373-9.373-9.373-24.569 0-33.941l10.823-10.823c9.562-9.562 25.133-9.34 34.419.492L480 342.118V160H292.451a24.005 24.005 0 0 1-16.971-7.029l-16-16C244.361 121.851 255.069 96 276.451 96H520c13.255 0 24 10.745 24 24v222.118l40.416-42.792c9.285-9.831 24.856-10.054 34.419-.492l10.823 10.823c9.372 9.372 9.372 24.569-.001 33.941zm-265.138 15.431A23.999 23.999 0 0 0 347.548 352H160V169.881l40.416 42.792c9.286 9.831 24.856 10.054 34.419.491l10.822-10.822c9.373-9.373 9.373-24.569 0-33.941L144.971 67.716c-9.373-9.373-24.569-9.373-33.941 0L10.343 168.402c-9.373 9.373-9.373 24.569 0 33.941l10.822 10.822c9.562 9.562 25.133 9.34 34.419-.491L96 169.881V392c0 13.255 10.745 24 24 24h243.549c21.382 0 32.09-25.851 16.971-40.971l-16.001-16z"/></svg>
                        </button>
                    </div>
                </div>
            </div>
        );
    }
}