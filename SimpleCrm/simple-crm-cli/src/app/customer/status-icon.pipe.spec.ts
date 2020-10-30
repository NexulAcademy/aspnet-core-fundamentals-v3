import { StatusIconPipe } from './status-icon.pipe';

fdescribe('StatusIconPipe', () => {
  it('create an instance', () => {
    const pipe = new StatusIconPipe();
    expect(pipe).toBeTruthy();
  });
  it('Prospect should result in online', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('Prospect');
    expect(x).toEqual('online');
  });
  it('prospect (lowercase) should result in online', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prospect');
    expect(x).toEqual('online');
  });
  it('prOspEct (mixed case) should result in online', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('prOspEct');
    expect(x).toEqual('online');
  });
  it('Purchased should result in money', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('Purchased');
    expect(x).toEqual('money');
  });
  it('purchased (lowercase) should result in money', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('purchased');
    expect(x).toEqual('money');
  });
  it('pUrchased (mixed case) should result in money', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('pUrchased');
    expect(x).toEqual('money');
  });
  it('empty string should result in fallback value', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform('');
    expect(x).toEqual('users');
  });
  it('null should result in fallback value', () => {
    const pipe = new StatusIconPipe();
    const x = pipe.transform(null);
    expect(x).toEqual('users');
  });
});
