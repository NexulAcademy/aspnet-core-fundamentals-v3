import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusIcon'
})
export class StatusIconPipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): unknown {
    if (value === 'Prospect') {
      return 'online';
    }
    if (value === 'Purchased') {
      return 'money';
    }
    return 'users';
  }
}
