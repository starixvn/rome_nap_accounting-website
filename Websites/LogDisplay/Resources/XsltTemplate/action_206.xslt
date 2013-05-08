<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>
    <xsl:template match="acc_customerupdagent_commission">
        <table class="tblDesc">
            <tr>
                <!--<td class="colW1">{lbl:custid}</td>
				<td class="colW2">
					<xsl:value-of select="@custid" />
				</td>-->

                <td class="colW1">{lbl:groupa}</td>
                <td class="colW2">
                    <xsl:value-of select="@groupa" />
                </td>

                <td class="colW1">{lbl:groupb}</td>
                <td class="colW2">
                    <xsl:value-of select="@groupb" />
                </td>
                <td class="colW1">{lbl:groupc}</td>
                <td class="colW2">
                    <xsl:value-of select="@groupc" />
                </td>
            </tr>
            <tr>
                <td class="colW1">{lbl:groupd}</td>
                <td class="colW2">
                    <xsl:value-of select="@groupd" />
                </td>
                <td class="colW1">{lbl:discountcs}</td>
                <td class="colW2">
                    <xsl:value-of select="@discountcs" />
                </td>
                <td class="colW1">{lbl:discount1x2}</td>
                <td class="colW2">
                    <xsl:value-of select="@discount1x2" />
                </td>
                <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:custid}</td>
                        <td class="colW2">

                            <xsl:value-of select="@custid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->
            </tr>
            <!--<tr>
               
                <td class="colW1">{lbl:UserID}</td>
				<td class="colW2">
					<xsl:value-of select="@userid" />
				</td>
                <xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>
              
            </tr>-->
            <tr>
                <td class="colW1">{lbl:SubAccName}</td>
                <td class="colW2">
                    <xsl:value-of select="@subaccname" />
                </td>
                <td class="colW1">{lbl:Language}</td>
                <td class="colW2">
                    <xsl:value-of select="@language" />
                </td>

                <td class="colTitle">{lbl:discountnumber}</td>
                <td class="colValue">
                  <xsl:value-of select="@discountnumber" />
                </td>

            </tr>
            <tr>
              <td class="colW1">{lbl:IP}</td>
              <td class="colW2">
                <xsl:value-of select="@ip" />
              </td>
                <td class="colW1">{lbl:ErrCode}</td>
                <td class="colW2">
                    <xsl:value-of select="@errcode" />
                </td>
                <td class="colW1">{lbl:ErrMsg}</td>
                <td class="colW2">
                    <xsl:value-of select="@errmsg" />
                </td>
                
            </tr>
        </table>
    </xsl:template>


</xsl:stylesheet>
