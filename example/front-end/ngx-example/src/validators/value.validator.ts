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

  // Comparision operator.
  private _operator: string;

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

  @Input('value-validator-operator')
  public get fnGetOperator(): string {
    return this._operator;
  }

  @Input('value-validator-operator')
  public set fnSetOperator(v: string) {
    this._operator = v;
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

    switch (this._operator){
      case '<':
        console.log(`Control value: ${controlValue} < Value: ${this._value} = ${controlValue < this._value}`);
        if (controlValue < this._value)
          return null;
        break;

      case '<=':
        console.log(`Control value: ${controlValue} <= Value: ${this._value} = ${controlValue <= this._value}`);
        if (controlValue <= this._value)
          return null;
        break;

      case '>=':
        console.log(`Control value: ${controlValue} >= Value: ${this._value} = ${controlValue >= this._value}`);
        if (controlValue >= this._value)
          return null;
        break;

      case '>':
        console.log(`Control value: ${controlValue} > Value: ${this._value} = ${controlValue > this._value}`);
        if (controlValue > this._value)
          return null;
        break;

      default:
        console.log(`Control value: ${controlValue} == Value: ${this._value} = ${controlValue == this._value}`);
        if (controlValue == this._value)
          return null;
        break;
    }

    return {
      'value-validation': true
    };
  }

  //#endregion
}
