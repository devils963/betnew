import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PricingtablesComponent } from './pricingtables.component';

describe('PricingtablesComponent', () => {
  let component: PricingtablesComponent;
  let fixture: ComponentFixture<PricingtablesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PricingtablesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PricingtablesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
