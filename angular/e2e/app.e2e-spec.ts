import { InventoryManagementSystemTemplatePage } from './app.po';

describe('InventoryManagementSystem App', function() {
  let page: InventoryManagementSystemTemplatePage;

  beforeEach(() => {
    page = new InventoryManagementSystemTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
