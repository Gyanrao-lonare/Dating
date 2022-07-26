import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginationResult } from "../_modal/pagination";

export function getPaginationHEaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
  }
  export function getPaginatedResults<T>(url, params , http : HttpClient) {
    const paginatedResult: PaginationResult<T> = new PaginationResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map((responce: any) => {
        paginatedResult.result = responce.body;
        if (responce.headers.get('pagination') !== null) {
          paginatedResult.pagination = JSON.parse(
            responce.headers.get('pagination')
          );
        }
        return paginatedResult;
      })
    );
  }