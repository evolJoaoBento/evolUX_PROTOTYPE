import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class RemotesService {

	constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) { }

	public getSidebar(): Observable<any> {
		return this.http.get<any[]>(this.baseUrl + 'sidebar/GetMain');
	}

	public getExpeditionType(): Observable<any> {
		return this.http.get<any[]>(this.baseUrl + 'expeditiontype');
	}

	public get(pre: string, path: string): Observable<any> {
		return this.http.get<any[]>(this.baseUrl + pre + "/" + path);
	}
}
