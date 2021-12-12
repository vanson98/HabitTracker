import { HabitTrackerTemplatePage } from './app.po';

describe('HabitTracker App', function() {
  let page: HabitTrackerTemplatePage;

  beforeEach(() => {
    page = new HabitTrackerTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
