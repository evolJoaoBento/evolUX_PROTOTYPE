export class NavigationPath {
  label: string;
  routerLink: string;

  constructor(label: string, router: string) {
    this.label = label;
    this.routerLink = router;
  }
}
