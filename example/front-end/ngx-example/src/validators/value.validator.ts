import {AbstractControl, NG_VALIDATORS, ValidationErrors, Validator} from '@angular/forms';
import {Directive, Input} from '@angular/core';

@Directive({
  selector: '[value-validator][ngModel]',
  // We add our directive to the list of existing validators
  providers: [
    {provide: NG_VALIDATORS, useExisting: ValueValidator, multi: true}
  ]
})

export class ValueValidator implements Validator {

  //#region Properties

  /*
  * Function for hooking data change.
  * */
  private _onChange: () => void;

  // Value of control.
  private _value: any;


  @Input('value-validator')
  public get fnGetValidatorValue(): any {
    return this._value;
  }

  @Input('value-validator')
  public set fnSetValidatorValue(v: any) {
    this._value = v;
    if (this._onChange)
      this._onChange();
  }

  //#endregion

  //#region Methods

  /*
  * Register validator on change.
  * */
  registerOnValidatorChange(fn: () => void): void {
    this._onChange = fn;
  }

  /*
  * Validate control value.
  * */
  validate(abstractControl: AbstractControl): ValidationErrors | null {
    // Get current value of control.
    let controlValue = abstractControl.value;
    console.log(`Value: ${this._value} == ${controlValue} == ${this._value === controlValue}`);
    if (this._value === controlValue)
      return null;

    return {
      'value-validation': true
    }
  }

  //#endregion
}
